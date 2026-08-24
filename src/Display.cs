using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Combatants;
using Returns;

namespace Frontend
{
    public class Display
    {
        private readonly BlockingCollection<string> _inputQueue = new();
        private GameWindow? _window;

        public Display()
        {
            using ManualResetEventSlim ready = new();

            Thread uiThread = new(() =>
            {
                _window = new GameWindow(_inputQueue);
                ready.Set();
                Application.Run(_window);
            });
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.IsBackground = true;
            uiThread.Start();

            ready.Wait();
        }

        /// <summary>
        /// Renders a Return to the screen: its message, plus the side panel
        /// and the top state bar. The panel shows whichever of Combatants/
        /// Equipment the Return set - Combatants wins if both are somehow
        /// set - and falls back to the player's own stats when neither is.
        /// The state bar shows stateName if one's given, and is otherwise
        /// left as whatever it was already showing.
        /// </summary>
        /// <param name="response">The Return to display - drives both the message and the side panel</param>
        /// <param name="player">Player whose info populates the side panel when there's nothing more specific to show</param>
        /// <param name="stateName">Name of the active state to show in the top bar - Game.cs passes this in directly, since it's a property of the state rather than something a Return carries</param>
        public void Render(Return response, Player player, string? stateName = null, string end = "\n")
        {
            if (stateName != null)
            {
                _window!.Invoke(() => _window.UpdateStateBar(stateName));
            }

            // Text prints first, side panel updates after - so a turn's
            // numbers don't jump to their post-turn value while the message
            // describing that turn is still typing out.
            PrintOut(response.Message + end);

            _window!.Invoke(() =>
            {
                if (response.Combatants is { Count: > 0 })
                {
                    _window.ShowCombatPanel(response.Combatants);
                }
                else if (response.Equipment == true)
                {
                    _window.ShowEquipmentPanel(player);
                }
                else
                {
                    _window.ShowPlayerPanel(player);
                }
            });
        }

        /// <summary>
        /// Prints an error message without touching the side panel - a parse
        /// or command error mid-combat (or mid-inventory) shouldn't wipe out
        /// whatever the panel was already showing. It just stays put until
        /// the next successful Render() explicitly changes it.
        /// </summary>
        public void RenderError(string message, string end = "\n")
        {
            PrintOut(message + end);
        }

        public string Input()
        {
            return _inputQueue.Take();
        }

        public void Exit()
        {
            Render(new Return("Are you sure you want to quit? (y/N)"), Player.Get);
            string response = Input().ToLower();
            if (response == "y")
            {
                Render(new Return("Exiting game..."), Player.Get);
                Environment.Exit(0);
            }
        }

        // Use 12 for game
        private void PrintOut(string message, int sleep = 12)
        {
            // Typing ahead while the previous message is still animating is
            // fine, but submitting mid-typewriter let half-printed output get
            // interleaved with the echoed command. Locking Enter out for the
            // duration (see OnInputKeyDown) keeps submission from landing
            // until the text actually finishes printing.
            _window!.Invoke(() => _window.SetTyping(true));

            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];

                // Rescrolling on every single character (instead of once per
                // completed line) is what caused the visible jitter - the view
                // was snapping to a recalculated scroll position dozens of times
                // a second instead of once.
                bool completesLine = c == '\n' || i == message.Length - 1;

                // We're on the game thread here - Invoke hops onto the UI thread,
                // runs the delegate there, and blocks us until it's done.
                _window!.Invoke(() => _window.AppendOutput(c.ToString(), completesLine));
                Thread.Sleep(sleep);
            }

            _window!.Invoke(() => _window.SetTyping(false));
        }

        private sealed class GameWindow : Form
        {
            private readonly RichTextBox _output;
            private readonly TextBox _input;
            private readonly BlockingCollection<string> _inputQueue;
            private readonly Label _sidePanelText;
            private readonly Panel _sidePanelContent;
            private int _sidePanelScroll = 0;
            private readonly Label _stateBar;
            private bool _isTyping = false;

            public GameWindow(BlockingCollection<string> inputQueue)
            {
                _inputQueue = inputQueue;

                Text = "Tomb of Ocura";
                Width = 900;
                Height = 650;
                BackColor = Color.Black;

                _output = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 11f),
                    BorderStyle = BorderStyle.None,
                    ScrollBars = RichTextBoxScrollBars.None,
                    TabStop = false
                };
                // RichTextBox doesn't expose DoubleBuffered publicly - it's a
                // protected Control property - so flip it on via reflection.
                // This is the standard fix for RichTextBox repaint flicker.
                typeof(Control).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, _output, [true]);

                // WinForms controls have no CSS-style inner padding. The trick is
                // to dock the real control to Fill inside a Panel that DOES have
                // a Padding, so the panel's inset reads as the control's padding.
                Panel outputPanel = new()
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black,
                    Padding = new Padding(12)
                };
                outputPanel.Controls.Add(_output);

                Font inputFont = new("Consolas", 11f);

                // Dock = Left/Fill stretch controls to the full row height, but
                // a single-line TextBox refuses that stretch (it silently keeps
                // its own font-based height and top-aligns instead) while a
                // Label happily stretches and then centers its text inside the
                // taller box - two different behaviors, hence the offset. A
                // TableLayoutPanel row leaves both controls at their natural
                // height instead, so they land on the same baseline.
                Label prompt = new()
                {
                    Text = ">",
                    AutoSize = true,
                    Margin = new Padding(0, 0, 2, 0),
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    Font = inputFont
                };

                _input = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Margin = Padding.Empty,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    Font = inputFont,
                    BorderStyle = BorderStyle.None
                };
                _input.KeyDown += OnInputKeyDown;

                // The output log is read-only and auto-scrolls itself - it should
                // never actually hold focus, whether from a click or a Tab press.
                // Bouncing focus straight back to the input box on Enter covers both.
                _output.Enter += (_, _) => _input.Focus();

                TableLayoutPanel inputRow = new()
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    BackColor = Color.Black,
                    Margin = Padding.Empty,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                inputRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                inputRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                inputRow.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                inputRow.Controls.Add(prompt, 0, 0);
                inputRow.Controls.Add(_input, 1, 0);

                // Same padding trick as the output panel, plus it's what holds
                // the prompt label and the textbox side by side.
                Panel inputPanel = new()
                {
                    Dock = DockStyle.Bottom,
                    Height = 44,
                    BackColor = Color.Black,
                    Padding = new Padding(12, 14, 12, 6)
                };
                inputPanel.Controls.Add(inputRow);

                // Side panel: shows player info by default, but Render() can
                // swap it to show combatants or equipment/inventory instead.
                // The label isn't docked - its Y position is nudged up/down by
                // hand to scroll, since there's no visible scrollbar to drag.
                _sidePanelText = new Label
                {
                    AutoSize = true,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 11f)
                };

                // This inner panel is what actually clips the label when it's
                // taller than the visible area - a plain child control can't
                // paint outside its parent's bounds, so anything scrolled past
                // the edge is simply cut off rather than spilling out.
                _sidePanelContent = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };
                _sidePanelContent.Controls.Add(_sidePanelText);
                _sidePanelContent.Resize += (_, _) => LayoutSidePanelText();
                _sidePanelContent.MouseWheel += OnSidePanelMouseWheel;
                _sidePanelText.MouseWheel += OnSidePanelMouseWheel;

                Panel sidePanel = new()
                {
                    Dock = DockStyle.Right,
                    Width = 230,
                    BackColor = Color.Black,
                    Padding = new Padding(12)
                };
                sidePanel.Controls.Add(_sidePanelContent);

                Color dividerColor = Color.FromArgb(60, 60, 60);
                Panel rightDivider = new() { Dock = DockStyle.Right, Width = 1, BackColor = dividerColor };
                Panel bottomDivider = new() { Dock = DockStyle.Bottom, Height = 1, BackColor = dividerColor };

                // Top bar: shows the active state's name (room, "Inventory",
                // "Combat", etc). Game.cs passes the name in on each Render -
                // it isn't carried on Return, since it comes from the state
                // itself rather than anything a state's Execute() produces.
                _stateBar = new Label
                {
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 14f, FontStyle.Bold)
                };

                Panel topBar = new()
                {
                    Dock = DockStyle.Top,
                    Height = 44,
                    BackColor = Color.Black,
                    Padding = new Padding(12, 6, 12, 6)
                };
                topBar.Controls.Add(_stateBar);

                Panel topDivider = new() { Dock = DockStyle.Top, Height = 1, BackColor = dividerColor };

                // The bar is scoped to the output column only (not the full
                // window width) by nesting it - along with outputPanel - inside
                // its own Fill container, rather than docking it to the Form
                // directly. If it docked to the Form, it'd resolve before the
                // side panel and stretch across the top of it too.
                Panel mainColumn = new()
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };
                mainColumn.Controls.Add(outputPanel);
                mainColumn.Controls.Add(topDivider);
                mainColumn.Controls.Add(topBar);

                // Controls are docked in reverse of add order (last added is on
                // top of the z-order and gets first pick of space), and the Fill
                // control must resolve last so it soaks up whatever remains.
                // Resolve order: inputPanel (Bottom) -> bottomDivider (Bottom,
                // spans full width above input) -> sidePanel (Right) ->
                // rightDivider (Right, between output and side panel) ->
                // mainColumn (Fill, whatever's left - topBar/topDivider then
                // resolve within it the same way). Add order is the reverse.
                Controls.Add(mainColumn);
                Controls.Add(rightDivider);
                Controls.Add(sidePanel);
                Controls.Add(bottomDivider);
                Controls.Add(inputPanel);

                Shown += (_, _) => _input.Focus();
                FormClosing += (_, _) => Environment.Exit(0);
            }

            private void OnInputKeyDown(object? sender, KeyEventArgs e)
            {
                if (e.KeyCode != Keys.Enter)
                {
                    return;
                }

                // Stop the Enter key from dinging/adding a newline into the box.
                e.SuppressKeyPress = true;

                // Typing ahead is fine, but submitting while the previous
                // message is still animating out interleaves the echoed
                // command with whatever's mid-typewriter. Just swallow Enter
                // until PrintOut says it's done.
                if (_isTyping)
                {
                    return;
                }

                string text = _input.Text;
                _input.Clear();

                // Echo what was typed into the log, same as a console transcript would.
                AppendOutput($"> {text}\n", scroll: true);

                _inputQueue.TryAdd(text);
            }

            public void SetTyping(bool typing)
            {
                _isTyping = typing;
            }

            public void UpdateStateBar(string stateName)
            {
                _stateBar.Text = stateName;
            }

            public void ShowPlayerPanel(Player p)
            {
                SetPanelText($"{p.Name}\nLevel {p.Level}\nXP: {p.PlayerExperience}\nHP: {p.CurrHealth}/{p.MaxHealth}\n\n{p.Stats.Status()}");
            }

            public void ShowCombatPanel(List<ICombatant> combatants)
            {
                SetPanelText(string.Join(
                    "\n\n",
                    combatants.Select(c => $"{c.Name}\nLvl {c.Level} {Regex.Replace(c.GetType().Name, @"(?<!^)(?=[A-Z])", " ")}\n{c.CurrHealth}/{c.MaxHealth}")));
            }

            public void ShowEquipmentPanel(Player p)
            {
                SetPanelText($"{p.Name}\nLevel {p.Level}\nXP: {p.PlayerExperience}\nHP: {p.CurrHealth}/{p.MaxHealth}\n\n{p.Equipment.Status()}");
            }

            // Whenever the panel's content is replaced outright (switching
            // between player/combat/equipment views), snap back to the top -
            // an old scroll offset from a totally different view is just confusing.
            private void SetPanelText(string text)
            {
                _sidePanelText.Text = text;
                _sidePanelScroll = 0;
                LayoutSidePanelText();
            }

            // Wraps the label to the panel's current width and re-applies the
            // scroll offset, clamping it so we can't scroll past either end.
            private void LayoutSidePanelText()
            {
                int width = Math.Max(_sidePanelContent.ClientSize.Width, 1);
                _sidePanelText.MaximumSize = new Size(width, 0);

                int minScroll = Math.Min(0, _sidePanelContent.ClientSize.Height - _sidePanelText.Height);
                _sidePanelScroll = Math.Clamp(_sidePanelScroll, minScroll, 0);
                _sidePanelText.Location = new Point(0, _sidePanelScroll);
            }

            private void OnSidePanelMouseWheel(object? sender, MouseEventArgs e)
            {
                _sidePanelScroll += Math.Sign(e.Delta) * _sidePanelText.Font.Height * 3;
                LayoutSidePanelText();
            }

            public void AppendOutput(string text, bool scroll)
            {
                _output.AppendText(text);

                // A long line (no '\n' inside it) can word-wrap past the bottom
                // of the visible view before it "completes", so text keeps
                // typing out below the fold and stays hidden until the line
                // finishes. Checking where the caret actually landed - and
                // scrolling the moment it drops below the viewport - catches
                // that case without forcing a scroll on every character (which
                // is what caused the jitter this flag was introduced to avoid).
                Point caretPos = _output.GetPositionFromCharIndex(Math.Max(_output.TextLength - 1, 0));
                bool caretBelowView = caretPos.Y >= _output.ClientSize.Height - _output.Font.Height;

                if (scroll || caretBelowView)
                {
                    _output.SelectionStart = _output.Text.Length;
                    _output.ScrollToCaret();
                }
            }
        }
    }
}

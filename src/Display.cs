using System.Collections.Concurrent;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Frontend
{
    /// <summary>
    /// Class that handles all front facing UI. Displays what we tell it plus what
    /// ever we decide in the future.
    ///
    /// The window runs on its own dedicated UI thread (WinForms owns that thread via
    /// Application.Run and its message loop). The game thread keeps calling
    /// Render/Input synchronously exactly like it did against the console - Render
    /// marshals text onto the UI thread, and Input blocks the game thread until the
    /// UI thread hands it a submitted line.
    /// </summary>
    public class Display
    {
        private readonly BlockingCollection<string> _inputQueue = new(boundedCapacity: 1);
        private readonly ManualResetEventSlim _windowReady = new();
        private GameWindow? _window;

        public Display()
        {
            Thread uiThread = new(StartUi)
            {
                IsBackground = true
            };
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();

            // Don't let the game thread touch _window before the UI thread has
            // actually created it.
            _windowReady.Wait();
        }

        private void StartUi()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _window = new GameWindow(_inputQueue);
            _windowReady.Set();

            // Blocks this thread forever, pumping window messages, until the
            // window closes.
            Application.Run(_window);
        }

        /// <summary>
        /// Renders information to the screen, mainly just a message atm, but
        /// will add more once we switch up display methods.
        /// </summary>
        /// <param name="message">String that will be displayed as main message after action</param>
        public void Render(string message, string end = "\n")
        {
            PrintOut(message + end);
        }

        public string Input()
        {
            return _inputQueue.Take();
        }

        public void Exit()
        {
            Render("Are you sure you want to quit? (y/N)");
            string response = Input().ToLower();
            if (response == "y")
            {
                Render("Exiting game...");
                Environment.Exit(0);
            }
        }

        // Use 12 for game
        private void PrintOut(string message, int sleep = 12)
        {
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
        }

        private sealed class GameWindow : Form
        {
            private readonly RichTextBox _output;
            private readonly TextBox _input;
            private readonly BlockingCollection<string> _inputQueue;

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
                    ForeColor = Color.LightGray,
                    Font = new Font("Consolas", 11f),
                    BorderStyle = BorderStyle.None
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
                    Height = 36,
                    BackColor = Color.Black,
                    Padding = new Padding(12, 6, 12, 6)
                };
                inputPanel.Controls.Add(inputRow);

                // Fill control goes in first - it claims whatever space is left
                // over after the other docked edges are carved out.
                Controls.Add(outputPanel);
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

                string text = _input.Text;
                _input.Clear();

                // Echo what was typed into the log, same as a console transcript would.
                AppendOutput($"> {text}\n", scroll: true);

                _inputQueue.TryAdd(text);
            }

            public void AppendOutput(string text, bool scroll)
            {
                _output.AppendText(text);
                if (scroll)
                {
                    _output.SelectionStart = _output.Text.Length;
                    _output.ScrollToCaret();
                }
            }
        }
    }
}

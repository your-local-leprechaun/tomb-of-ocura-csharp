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
            foreach (char c in message)
            {
                // We're on the game thread here - Invoke hops onto the UI thread,
                // runs the delegate there, and blocks us until it's done.
                _window!.Invoke(() => _window.AppendOutput(c.ToString()));
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

                _input = new TextBox
                {
                    Dock = DockStyle.Bottom,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 11f),
                    BorderStyle = BorderStyle.FixedSingle
                };
                _input.KeyDown += OnInputKeyDown;

                Controls.Add(_output);
                Controls.Add(_input);

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
                AppendOutput($"> {text}\n");

                _inputQueue.TryAdd(text);
            }

            public void AppendOutput(string text)
            {
                _output.AppendText(text);
                _output.SelectionStart = _output.Text.Length;
                _output.ScrollToCaret();
            }
        }
    }
}

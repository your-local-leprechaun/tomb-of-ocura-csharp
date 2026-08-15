

namespace Frontend
{
    public class Display
    {

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
            Console.Write("> ");
            string input = Console.ReadLine() ?? "";
            return input;
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
                Console.Write(c);
                Thread.Sleep(sleep);
            }
        }
    }
}

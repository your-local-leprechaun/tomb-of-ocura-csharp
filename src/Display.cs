// using System.Threading;

namespace Frontend
{
    /// <summary>
    /// Class that handles all front facing UI. Displays what we tell it plus what
    /// ever we decide in the future. 
    /// </summary>
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
            PrintOut("Are you sure you want to quit? (y/N)");
            string response = Input().ToLower();
            while (true)
            {
                if (response != "y")
                {
                    return;
                }
                
            }

        }

        // Use 12 for game
        private void PrintOut(string message, int sleep = 0)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(sleep);
            }
        }
    }
}
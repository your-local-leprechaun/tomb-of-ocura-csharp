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
        public void Render(string message)
        {
            PrintOut(message);
        }

        public string Input()
        {
            Console.Write("> ");
            string input = Console.ReadLine() ?? "";
            return input;
        }

        private void PrintOut(string message, int sleep = 12)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(sleep);
            }
            Console.WriteLine();
        }
    }
}
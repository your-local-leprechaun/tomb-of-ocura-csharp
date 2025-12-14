/*
This script is meant to be the main file to run that starts the rest of the program. By calling
this, the program as a whole will start
*/
using Utilities;

namespace Main
{
    class MainRun()
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            General.PrintOut("Hello World");
            Console.ResetColor();
        }
    }
}
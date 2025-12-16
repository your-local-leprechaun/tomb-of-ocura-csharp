/*
This script is meant to be the main file to run that starts the rest of the program. By calling
this, the program as a whole will start
*/
using Utilities;
using TombOfOcura.Rooms;

namespace Main
{
    class MainRun()
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            General.PrintOut("Main program starting...");
            Console.ResetColor();

            // Testing Rooms
            IRoom room = new Room1();
            General.PrintOut(room.Name);
            General.PrintOut(room.Description);

            General.PrintOut("Choices:");
            List<string> choices = room.GetChoices();
            foreach (string choice in choices)
            {
                General.PrintOut(choice);
            }
        }   
    }
}
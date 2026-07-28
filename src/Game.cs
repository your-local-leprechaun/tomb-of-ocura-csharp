using Parser;
using State;
using Commands;

namespace Game
{
    public class Game
    {
        // IState _activeState = new Room.ExampleRoom();
        Parser.Parser parser = new Parser.Parser();

        /// <summary>
        /// Everything lives in this loop. We change states here,
        /// everything is here. This loop ends, and so does the game.
        /// </summary>
        public void GameLoop()
        {
            while (true)
            {
                // Get Player input
                Console.Write("> ");
                string input = Console.ReadLine() ?? "";

                // Parse the command into a command that follows the order of
                // VERB [ADJECTIVE] NOUN
                try
                {
                    Command command = parser.ParseInput(input);

                    // Check for basic commands (Exit)
                    if (command.Verb == "exit")
                    {
                        Console.WriteLine("Quitting Game...");
                        Environment.Exit(0);
                    }

                    // Call command to the active state, and recieve information back
                    // Dictionary<string, object> response = _activeState.Command(command);

                    // Check for updates that need to be made

                    // Display message
                    Console.WriteLine($"Verb: {command.Verb} Adjective: {command.Adjective} Noun: {command.Noun}");
                }
                catch (ParseException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

            }
        }
    }
}
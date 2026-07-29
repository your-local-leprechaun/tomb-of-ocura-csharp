using Parser;
using State;
using Commands;
using Returns;

namespace Game
{
    public class Game
    {
        IState _activeState = Testing.TestRoom.Get;
        Parser.Parser parser = new Parser.Parser();
        Frontend.Display Display = new Frontend.Display();

        /// <summary>
        /// Everything lives in this loop. We change states here,
        /// everything is here. This loop ends, and so does the game.
        /// </summary>
        public void GameLoop()
        {
            while (true)
            {
                // Get Player input
                string input = Display.Input();

                // Parse the command into a command that follows the order of
                // VERB [ADJECTIVE] NOUN
                try
                {
                    Command command = parser.ParseInput(input);

                    // Check for basic commands (Exit)
                    if (command.Verb == "exit")
                    {
                        Display.Render("Exiting game...");
                        Environment.Exit(0);
                    }

                    // Call command to the active state, and recieve information back
                    Return response = _activeState.Execute(command);

                    // Display message
                    Display.Render(response.Message);

                    // Update state if needed
                    if (response.UpdateState != null)
                    {
                        _activeState = response.UpdateState;
                        response = _activeState.Activate();
                        Display.Render(response.Message);
                    }
                }
                catch (ParseException e)
                {
                    Display.Render(e.Message);
                    continue;
                }

            }
        }
    }
}
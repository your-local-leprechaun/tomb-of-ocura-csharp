using Parser;
using State;
using Commands;
using Returns;

namespace Main
{
    public class Game
    {
        // IState _activeState = new MainMenu();
        IState _activeState = Rooms.Room3.Get;
        IState? _previousState = null;
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

                    Return response;

                    // Check for basic commands (Exit)
                    if (command.Verb == "exit")
                    {
                        Quit();
                        continue;
                    }
                    else if (command.Verb == "status" && command.Noun == "player")
                    {
                        Player player = Player.Get;
                        response = player.Status();
                    }
                    else
                    {
                        // Call command to the active state, and recieve information back
                        response = _activeState.Execute(command);
                    }

                    // Display message
                    Display.Render(response.Message);

                    // Update state if needed
                    if (response.UpdateState != null)
                    {
                        _previousState = _activeState;
                        _activeState = response.UpdateState;
                        response = _activeState.Activate();
                        Display.Render(response.Message);
                    }

                    // Return to Previous State
                    if (response.Previous == true && _previousState != null)
                    {
                        var temp = _activeState;
                        _activeState = _previousState;
                        _previousState = temp;
                        response = _activeState.Activate();
                        Display.Render(response.Message);
                    }
                }
                catch (ParseException e)
                {
                    Display.Render(e.Message);
                    continue;
                }
                catch (CommandException e)
                {
                    Display.Render(e.Message);
                    continue;
                }
            }
        }

        public void Start()
        {
            Return response = _activeState.Activate();
            Display.Render(response.Message);

            GameLoop();
        }

        public void Quit()
        {
            Display.Render("Are you sure you want to quit? (y/N)");
            string response = Display.Input().ToLower();
            while (true)
            {
                if (response != "y")
                {
                    return;
                }
                Display.Render("Exiting game...");
                Environment.Exit(0);
            }
        }
    }
}
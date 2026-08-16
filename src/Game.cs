using Parser;
using State;
using Commands;
using Returns;
using Rooms;

namespace Main
{
    public class Game
    {
        // IState _activeState = new MainMenu();
        IState _activeState = Rooms.Room2.Get;
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
                    else if (command == new Command("status", "player"))
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
                    Display.Render(response, Player.Get, _activeState.Name);

                    // Update state if needed (loop in case Activate() itself
                    // triggers another transition, e.g. entering a room with enemies)
                    while (response.UpdateState != null)
                    {
                        _previousState = _activeState;
                        _activeState = response.UpdateState;
                        response = _activeState.Activate();
                        Display.Render(response, Player.Get, _activeState.Name);
                    }

                    // Return to Previous State
                    if (response.Previous == true && _previousState != null)
                    {
                        var temp = _activeState;
                        _activeState = _previousState;
                        _previousState = temp;
                        response = _activeState.Activate();
                        Display.Render(response, Player.Get, _activeState.Name);
                    }

                    // Set Checkpoint
                    if (response.Checkpoint != null)
                    {
                        RoomRegistry.UpdateCheckpoint(_activeState as IRoom ?? null);
                    }
                }
                catch (ParseException e)
                {
                    Display.RenderError(e.Message);
                    continue;
                }
                catch (CommandException e)
                {
                    Display.RenderError(e.Message);
                    continue;
                }
            }
        }

        public void Start()
        {
            Return response = _activeState.Activate();
            Display.Render(response, Player.Get, _activeState.Name);

            while (response.UpdateState is not null)
            {
                _previousState = _activeState;
                _activeState = response.UpdateState;
                response = _activeState.Activate();
                Display.Render(response, Player.Get, _activeState.Name);
            }

            GameLoop();
        }

        public void Quit()
        {
            Display.Render(new Return("Are you sure you want to quit? (y/N)"), Player.Get);
            string response = Display.Input().ToLower();
            while (true)
            {
                if (response != "y")
                {
                    return;
                }
                Display.Render(new Return("Exiting game..."), Player.Get);
                Environment.Exit(0);
            }
        }
    }
}
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
        IState _activeState = Rooms.Room6.Get;
        IState? _previousState = null;
        Parser.Parser parser = new Parser.Parser();
        Frontend.Display Display = new Frontend.Display();

        /// <summary>
        /// Everything lives in this loop. We change states here,
        /// everything is here. This loop ends, and so does the game.
        /// </summary>
        public void GameLoop()
        {
            bool skipInput = false;
            while (true)
            {
                try
                {
                    Command command = skipInput
                        ? new Command("skip", "input")
                        : parser.ParseInput(Display.Input());

                    Return response;

                    // Check for basic commands
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
                    else if (command == new Command("save", "game"))
                    {
                        response = new Return(SaveManager.Save());
                    }
                    else if (command == new Command("load", "game"))
                    {
                        response = new Return(SaveManager.Load());
                    }
                    else
                    {
                        response = _activeState.Execute(command);
                    }


                    // Call to active State with command
                    Display.Render(response);

                    // Check through all possible return conditions
                    while (response.UpdateState != null)
                    {
                        _previousState = _activeState;
                        _activeState = response.UpdateState;
                        response = _activeState.Activate();
                        Display.Render(response, _activeState.Name);
                    }

                    // Return to Previous State
                    if (response.Previous == true && _previousState != null)
                    {
                        var temp = _activeState;
                        _activeState = _previousState;
                        _previousState = temp;
                        response = _activeState.Activate();
                        Display.Render(response, _activeState.Name);
                    }

                    // Set Checkpoint
                    if (response.Checkpoint != null)
                    {
                        RoomRegistry.UpdateCheckpoint(_activeState as IRoom ?? null);
                    }

                    skipInput = response.SkipInput == true;
                }
                catch (ParseException e)
                {
                    Display.RenderError(e.Message);
                    skipInput = false;
                }
                catch (CommandException e)
                {
                    Display.RenderError(e.Message);
                    skipInput = false;
                }
            }
        }

        public void Start()
        {
            Return response = _activeState.Activate();
            Display.Render(response, _activeState.Name);

            while (response.UpdateState is not null)
            {
                _previousState = _activeState;
                _activeState = response.UpdateState;
                response = _activeState.Activate();
                Display.Render(response, _activeState.Name);
            }

            GameLoop();
        }

        public void Quit()
        {
            Display.Render(new Return("Are you sure you want to quit? (y/N)"));
            string response = Display.Input().ToLower();
            while (true)
            {
                if (response != "y")
                {
                    return;
                }
                Display.Render(new Return("Exiting game..."));
                Environment.Exit(0);
            }
        }
    }
}
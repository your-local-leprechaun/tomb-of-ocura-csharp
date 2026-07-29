
using Commands;
using Returns;
using State;

public class MainMenu : IState
{
    public Return Execute(Command command)
    {
        if (command.Verb == "start" && command.Noun == "game")
        {
            return new Return("\nBeginning your journey into Ocura...", Testing.TestRoom.Get);
        }
        return new Return("--Unknown Command--");
    }

    public Return Activate()
    {
        return new Return("The Tomb of Ocura\n\nStart Game\nQuit");
    }
}
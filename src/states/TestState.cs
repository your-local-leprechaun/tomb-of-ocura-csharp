using Commands;
using Returns;
using State;

namespace Testing
{
    public class TestRoom : IState
    {
        public Return Execute(Command command)
        {
            if (command.Verb == "switch" && command.Noun == "state")
            {
                return new Return("You switch states to TestRoom2", new TestRoom2());
            }
            return new Return("This is the test room");
        }

        public Return Activate()
        {
            return new Return("This room has just started");
        }
    }

    public class TestRoom2 : IState
    {
        public Return Execute(Command command)
        {
            if (command.Verb == "switch" && command.Noun == "state")
            {
                return new Return("You switch states to TestRoom", new TestRoom());
            }
            return new Return("This is the second test room");
        }

        public Return Activate()
        {
            return new Return("This room has just started");
        }
    }
}
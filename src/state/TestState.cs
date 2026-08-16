using Basic;
using Commands;
using Returns;
using State;

namespace Testing
{
    public sealed class TestRoom : Singleton<TestRoom>, IState
    {
        private TestRoom() {}

        public static TestRoom Get => Instance;

        public string Name => "Test Room";

        public Return Execute(Command command)
        {
            if (command.Verb == "switch" && command.Noun == "state")
            {
                return new Return("You switch states to TestRoom2", TestRoom2.Get);
            }
            return new Return("This is the test room");
        }

        public Return Activate()
        {
            return new Return("This room has just started");
        }
    }

    public class TestRoom2 : Singleton<TestRoom2>, IState
    {
        private TestRoom2() {}

        public static TestRoom2 Get => Instance;

        public string Name => "Test Room 2";

        bool testing = false;

        public Return Execute(Command command)
        {
            if (command.Verb == "switch" && command.Noun == "state")
            {
                return new Return("You switch states to TestRoom", TestRoom.Get);
            }
            else if (command.Verb == "grab" && command.Noun == "sword")
            {
                testing = true;
                return new Return("You have changed this room. Type command to test.");
            }
            if (testing == true)
            {
                return new Return("This room has been changed!");
            }
            return new Return("This is the second test room");
        }

        public Return Activate()
        {
            return new Return("This room has just started");
        }
    }
}
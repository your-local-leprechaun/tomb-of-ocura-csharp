
using TombOfOcura.Item;

namespace TombOfOcura.Rooms
{
    public class Room1 : Room
    {
        private static Room1? _instance;

        public static Room1 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Room1();
                }
                return _instance;
            }
        }

        private Room1() : base("Jail Cell", "You are in a dimly lit jail cell.")
        {
            Choices.Add("Grab Key");
            IItem key = new Key();
            Items.Add(key);
        }

        public override string HandleCommand(string command)
        {
            return "you can't do that here";
        }

        public override void RespawnEnemies()
        {
            // No enemies in this room
        }
    }
}
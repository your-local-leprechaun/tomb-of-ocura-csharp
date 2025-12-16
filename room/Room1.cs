
namespace TombOfOcura.Rooms
{
    public class Room1 : RoomBase
    {
        public Room1() : base("Jail Cell", "You are in a dimly lit jail cell.")
        {
            Choices.Add("Grab Key");
            Items.Add("Key");    
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
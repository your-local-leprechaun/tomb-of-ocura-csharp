using Commands;
using Frontend;
using Returns;

namespace Rooms
{
    public class Hallway1 : RoomBase<Hallway1>
    {
        private Hallway1() : base(
            "Hallway",
            "The long hallway stretches out before you. To the south is the symbol room. To the north in front of you is a room with movement. You might have to fight something."
        )
        {
            // Add OG Choices
            RegisterHandler(new Command("move", "north"), MoveNorth);
            RegisterHandler(new Command("move", "south"), MoveSouth);
        }

        // Custom Choice Methods
        private Return MoveNorth()
        {
            // Move to Room 5
            return new Return("You walk to the northern room.", Room5.Get);
        }
        private Return MoveSouth()
        {
            // Move to Symbol Room
            return new Return("You walk to the southern room.", Room2.Get);
        }
    }
}
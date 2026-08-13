using Commands;
using Returns;

namespace Rooms
{
    public class SecretPassage1 : RoomBase<SecretPassage1>
    {
        private SecretPassage1() : base(
            "Secret Passage",
            "The secret passage way makes a hard turn twice. To the north is a room with three green humanoids. To the south is the painting room."
        )
        {
            // Add OG Choices
            RegisterHandler(new Command("move", "north"), MoveNorth);
            RegisterHandler(new Command("move", "south"), MoveSouth);
        }

        // Custom Choice Methods
        private Return MoveNorth()
        {
            // Move to Room 6
            return new Return("You walk to the northern room.", Room6.Get);
        }
        private Return MoveSouth()
        {
            // Move to Painting Room
            return new Return("You walk to the southern room.", Room3.Get);
        }
    }
}
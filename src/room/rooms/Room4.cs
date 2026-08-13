using Commands;
using Items;
using Returns;

namespace Rooms
{
    public class Room4 : RoomBase<Room4>
    {
        private Room4() : base(
            "Treasure Room",
            "The well lit room contains a chest, that sits unopened on the south wall in front of two wall torches. To the north is the Painting Room."
        )
        {
            // OG Choices
            RegisterHandler(new Command("move", "north"), MoveNorth);
            RegisterHandler(new Command("open", "chest"), OpenChest);

            // AddItem(new RedRing());
        }

        // Choice Methods
        private Return MoveNorth()
        {
            return new Return("Move to the north", Room3.Get);
        }
        private Return OpenChest()
        {
            // Collect Red Ring
            // Update Description with open chest
            UpdateDescription("The well lit room contains an open chest agaist the south wall in front of two wall torches. To the north is the Painting Room.");
            // Take away choice
            UnregisterHandler(new Command("open", "chest"));

            return new Return("You open the chest and find a small red ring. The ring will fit on your finger.");
        }
    }
}
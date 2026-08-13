using Commands;
using Enemies;
using Items.Equipment;
using Returns;

namespace Rooms
{
    public class Room6 : RoomBase<Room6>
    {
        private Room6() : base(
            "Staircase",
            "A room with a staircase leading down to the north, with a room to the west and a small entrance to the east."
        )
        {
            // Add OG Choices
            RegisterHandler(new Command("move", "north"), MoveNorth);
            RegisterHandler(new Command("move", "west"), MoveWest);

            // Add OG Items
            AddItem(new BrokenBreastplate());

            // Add Enemies
            AddEnemy(new Skeleton("Skele"));
            AddEnemy(new Skeleton("Ashey"));
            AddEnemy(new SkeletonCommander("White Bone"));
        }

        // Custom Choice Methods
        private Return MoveNorth()
        {
            return new Return("You begin down the stair case.\n--Floor 2--");
        }

        private Return MoveWest()
        {
            return new Return("You walk to the room to the west.", Room5.Get);
        }
    }
}
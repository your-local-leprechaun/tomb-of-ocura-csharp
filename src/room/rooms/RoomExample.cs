using Commands;
using Items;
using Returns;

namespace Rooms
{
    public class RoomExample : RoomBase<Room2>
    {
        private RoomExample()
        {
            // RoomName = "Symbol Room";

            // Description = "Something Something description here"

            // OG Choices
            // RegisterHandler(new Command("move", "north"), MoveNorth);

            // AddItem(new RedKey());
        }

        public override Return Activate()
        {
            return new Return(Description);
        }

        // Choice Methods
        // private Return MoveNorth()
        // {
        //     return new Return("Move to the north");
        // }
    }
}
using Commands;
using Items;
using Returns;

namespace Rooms
{
    public class Room1 : RoomBase<Room1>
    {
        private Room1()
        {
            RoomName = "Jail Cell";

            UpdateDescription("You awake in a jail cell. There is a cell door in front of you and a key on the floor next to the bed.");

            RegisterHandler(new Command("grab", "key"), GrabKey);
            RegisterHandler(new Command("open", "door"), OpenDoor);
            // AddChoice("use bed");

            AddItem(new BasicKey());
            AddItem(new Note12());
        }

        private Return GrabKey()
        {
            UpdateDescription("You stand in a jail cell. There is a cell door in front of you and a bed behind you.");

            CollectItem<BasicKey>();
            UnregisterHandler(new Command("grab", "key"));

            return new Return("You pickup the key");
        }

        private Return OpenDoor()
        {
            if (Inventory.Get.Contains<BasicKey>())
            {
                //Open Door
                UpdateDescription("You are in a brick room with an opened jail cell. There is a bed, a pile of hay, and a room to the north.");
                RegisterHandler(new Command("move", "north"), MoveNorth);
                RegisterHandler(new Command("check", "hay"), CheckHay, showChoice: false);
                UnregisterHandler(new Command("open", "door"));
                Inventory.Get.RemoveItem<BasicKey>();

                return new Return("You use the basic key, and push open the cell door. There's a pile of hay and a room to the north.");
            }
            // Try door, but locked
            return new Return("You shake the door trying to open it, but it seems to be locked.");
        }

        private Return MoveNorth()
        {
            // Move to Room 2
            return new Return("You walk through the northern doorway.", Room2.Get);
        }

        private Return CheckHay()
        {
            UpdateDescription("You are in a brick room with an opened jail cell. There is a bed in the cell and a room to the north.");
            UnregisterHandler(new Command("check", "hay"));
            CollectItem<Note12>();
            return new Return("Looking through the pile of hay, you find a small note.");
        }
    }
}
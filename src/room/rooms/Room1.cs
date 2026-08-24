using Commands;
using Items;
using Returns;

namespace Rooms
{
    public class Room1 : RoomBase<Room1>
    {
        private Room1() : base
        (
            "Jail Cell",
            "You awake in a jail cell. There is a cell door in front of you and a key on the floor next to the bed."
        )
        {
            RegisterHandler(new Command("grab", "key"), GrabKey);
            RegisterHandler(new Command("open", "door"), OpenDoor);
            RegisterHandler(new Command("use", "bed"), UseBed);

            AddItem(new BasicKey());
            AddItem(new Note12());
        }

        private Return GrabKey()
        {
            UpdateDescription("You stand in a jail cell. There is a cell door in front of you and a bed behind you.");

            CollectItem<BasicKey>();
            UnregisterHandler(new Command("grab", "key"));
            MarkDone();

            return new Return("You pickup the key");
        }

        private Return OpenDoor()
        {
            // The key check is normally what gates this - but during Replay() the
            // key's already been consumed (it was removed from inventory the first
            // time this ran), so IsDone stands in for "we already know this happened"
            // once the flag's been loaded from a save.
            if (Inventory.Get.Contains<BasicKey>() || IsDone(nameof(OpenDoor)))
            {
                //Open Door
                UpdateDescription("You are in a brick room with an opened jail cell. There is a bed, a pile of hay, and a room to the north.");
                RegisterHandler(new Command("move", "north"), MoveNorth);
                RegisterHandler(new Command("check", "hay"), CheckHay, showChoice: false);
                UnregisterHandler(new Command("open", "door"));
                Inventory.Get.RemoveItem<BasicKey>();
                MarkDone();

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
            MarkDone();
            return new Return("Looking through the pile of hay, you find a small note.");
        }

        /// <summary>
        /// Re-fires whichever of the above already completed, in the order they
        /// depend on each other, so the room's handlers/description end up back
        /// where they were without needing to serialize either directly.
        /// </summary>
        protected override void Replay()
        {
            if (IsDone(nameof(GrabKey))) GrabKey();
            if (IsDone(nameof(OpenDoor))) OpenDoor();
            if (IsDone(nameof(CheckHay))) CheckHay();
        }

        private Return UseBed()
        {
            return new Return("You lay down in bed.", new BedState());
        }
    }
}
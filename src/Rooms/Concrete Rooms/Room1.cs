using Commands;
using Items;
using Returns;

namespace Rooms
{
    public class Room1 : RoomBase<Room1>
    {
        private bool _secret = false;

        private Room1()
        {
            UpdateDescription("You awake in a jail cell. There is a cell door in front of you and a key on the floor next to the bed.");

            AddChoice("grab key");
            AddChoice("use bed");
            AddChoice("open door");

            AddItem(new BasicKey());
        }

        public override Return Activate()
        {
            return new Return(Description);
        }

        protected override Return? CustomChoices(Command command)
        {
            if (command.Verb == "grab" && command.Noun == "key" && ContainsChoice("grab key"))
            {
                // We grab the key
                UpdateDescription("You stand in a jail cell. There is a cell door in front of you and a bed behind you.");
                IItem key = new BasicKey();
                Inventory.Get.AddItem(key);
                RemoveItem(key);
                RemoveChoice("grab key");

                return new Return("You pickup the key");
            }
            else if (command.Verb == "open" && command.Noun == "door")
            {
                if (Inventory.Get.Contains<BasicKey>())
                {
                    //Open Door
                    UpdateDescription("You are in a brick room with an opened jail cell. There is a bed, a pile of hay, and a room to the north.");
                    AddChoice("move north");
                    RemoveChoice("open door");
                    Inventory.Get.RemoveItem<BasicKey>();

                    return new Return("You use the basic key, and push open the cell door. There's a pile of hay and a room to the north.");
                }
                // Try door
                return new Return("You shake the door trying to open it, but it seems to be locked.");
            }
            else if (command.Verb == "move" && command.Noun == "north" && ContainsChoice("move north"))
            {
                // Move to Room 2
                return new Return("You walk through the northern doorway.", new MainMenu());
            }
            else if (command.Verb == "check" && command.Noun == "hay" && _secret == false && ContainsChoice("move north"))
            {
                // Find hidden note
                _secret = true;
                return new Return("Looking through the pile of hay, you find a small note.\n-Note 1 added to Inventory-");
            }

            return null;
        }
    }
}
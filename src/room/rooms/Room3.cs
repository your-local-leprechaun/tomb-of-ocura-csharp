using Commands;
using Items;
using Items.Equipment;
using Returns;

namespace Rooms
{
    public class Room3 : RoomBase<Room3>
    {
        private Room3() : base(
            "Painting Room",
            ""
        )
        {
            RebuildDescription();

            // OG Choices
            RegisterHandler(new Command("move", "west"), MoveWest);
            RegisterHandler(new Command("fix", "painting"), FixPainting, showChoice: false);
            RegisterHandler(new Command("open", "door"), OpenDoor);
            RegisterHandler(new Command("grab", "sword"), GrabSword);
            RegisterHandler(new Command("grab", "sword", "rusty"), GrabSword);

            // Items
            AddItem(new RustySword());
        }

        private readonly Dictionary<string, string> _descriptionFragments = new()
        {
            { "sword", ", with a rusty sword on the floor" },
            { "painting", "crooked painting on" },
            { "red door", "a red door" }
        };

        private void SetFragment(string key, string value)
        {
            _descriptionFragments[key] = value;
            RebuildDescription();
        }

        private void RebuildDescription()
        {
            string description =
                    $"The large room has moss growing through the bricks{_descriptionFragments["sword"]}. " +
                    $"There is a {_descriptionFragments["painting"]} the east wall. " +
                    $"To the south is {_descriptionFragments["red door"]} lit by two torches. To the west is the room with the symbol.";
            
            UpdateDescription(description);
        }

        // Choice Methods
        private Return MoveWest()
        {
            return new Return("You exit the room to the west.", Room2.Get);
        }
        private Return MoveEast()
        {
            return new Return("You walk into the secret passageway.", SecretPassage1.Get);
        }
        private Return MoveSouth()
        {
            return new Return("You walk through the red door.", Room4.Get);
        }
        private Return FixPainting()
        {
            UnregisterHandler(new Command("fix", "painting"));
            RegisterHandler(new Command("move", "east"), MoveEast);

            SetFragment("painting", "secret passageway in");

            return new Return("Fixing the crooked painting, you hear a small click behind it. The painting swings out, revealing a secret passageway behind it.");
        }
        private Return OpenDoor()
        {
            if (!Inventory.Get.Contains<RedKey>())
            {
                return new Return("You try the handle on the red door, but it's locked. Maybe there's a key...");
            }

            UnregisterHandler(new Command("open", "door"));
            RegisterHandler(new Command("move", "south"), MoveSouth);

            SetFragment("red door", "an open red door");

            return new Return("You use the Red Key on the door, and push it open. A new room sits beyond it.");
        }
        private Return GrabSword()
        {
            // Collect Rusty Sword
            CollectItem<RustySword>();

            // Remove Both Handlers
            UnregisterHandler(new Command("grab", "sword"));
            UnregisterHandler(new Command("grab", "sword", "rusty"));

            // Set Fragment to nothing
            SetFragment("sword", "");

            return new Return("You pick up the rusty sword. It doesn't seem powerful, but it'll work.");
        }
    }
}
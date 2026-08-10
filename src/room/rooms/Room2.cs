using Commands;
using Items;
using Returns;

namespace Rooms
{
    public class Room2 : RoomBase<Room2>
    {
        private Room2() : base(
            "Symbol Room",
            ""
        )
        {
            RebuildDescription();

            // OG Choices
            RegisterHandler(new Command("move", "north"), MoveNorth);
            RegisterHandler(new Command("move", "east"), MoveEast);
            RegisterHandler(new Command("move", "south"), MoveSouth);
            RegisterHandler(new Command("check", "symbol"), CheckSymbol);
            RegisterHandler(new Command("light", "candles"), LightCandles, showChoice: false);
            RegisterHandler(new Command("check", "chairs"), CheckChairs, showChoice: false);

            AddItem(new RedKey());
            AddItem(new Trash());
        }

        // 
        private readonly Dictionary<string, string> _descriptionFragments = new()
        {
            { "symbol", "a symbol drawn in blood" },
            { "candles", "unlit candles" },
            { "eastExit", "a hallway" },
            { "northExit", "a room" }
        };

        private void SetFragment(string key, string value)
        {
            _descriptionFragments[key] = value;
            RebuildDescription();
        }

        private void RebuildDescription()
        {
            string description = 
                $"The room is filled with chairs. " +
                $"On the floor in the center of the room is {_descriptionFragments["symbol"]} " +
                $"with {_descriptionFragments["candles"]} around it. " +
                $"To the east is {_descriptionFragments["eastExit"]}. " +
                $"To the north is {_descriptionFragments["northExit"]}. " +
                $"To the south is the Jail Cell.";

            UpdateDescription(description);
        }

        // Choice Methods
        private Return MoveNorth()
        {
            return new Return("You walk to the north.", Hallway1.Get);
        }
        private Return MoveEast()
        {
            return new Return("You walk to the east.", Room3.Get);
        }
        private Return MoveSouth()
        {
            return new Return("You walk to the jail cell.", Room1.Get);
        }
        private Return CheckSymbol()
        {
            // Update symbol fragment.
            SetFragment("symbol", "the Symbol of the Bloodmoon");

            UnregisterHandler(new Command("check", "symbol"));
            RegisterHandler(new Command("check", "symbol"), CheckSymbolAgain);

            // Return lore
            return new Return("Taking a closer look at the symbol on the floor, you find a memory sneaking in. This symbol is for something called 'The Bloodmoon'. You struggle to remember more anything else");
        }
        private Return CheckSymbolAgain()
        {
            return new Return("'The Symbol of the Bloodmoon'...");
        }
        private Return LightCandles()
        {
            CollectItem<RedKey>();

            UnregisterHandler(new Command("light", "candles"));
            SetFragment("candles", "candles aflame");
            return new Return("Stepping towards the candles, you lean in to light them. Before you can, the candles begin to burn. As they burn higher and build a large flame, you seem them conjoin. Inside the flame is a key. Reaching into the flame, you find your hand perfectly fine, and take the key. The candles simmer down to a normal flame.");
        }
        private Return CheckChairs()
        {
            CollectItem<Trash>();
            UnregisterHandler(new Command("check", "chairs"));
            return new Return("You get begin looking at the chairs, possibly looking for secrets. Instead, you find some trash. You take it anyways.");            
        }
    }
}
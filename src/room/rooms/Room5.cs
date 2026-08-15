using Commands;
using Enemies;
using Items.Equipment;
using Returns;

namespace Rooms
{
    public class Room5 : RoomBase<Room5>
    {
        private Room5() : base(
            "Armory",
            "A room that was once an armory, there are pieces of armor on many manaquins, with a few other pieces laying about. The only piece that looks to fit you is a rusty breastplate. To the south is the hallway. To the east is a room."
        )
        {
            // Add OG Choices
            RegisterHandler(new Command("move", "south"), MoveSouth);
            RegisterHandler(new Command("move", "east"), MoveEast);
            RegisterHandler(new Command("grab", "breastplate"), GrabBreastplate);
            RegisterHandler(new Command("grab", "breastplate", "broken"), GrabBreastplate);

            // Add OG Items
            AddItem(new BrokenBreastplate());

            // Add Enemies in Respawn function
            Respawn();
        }

        // Custom Choice Methods
        private Return MoveSouth()
        {
            return new Return("You walk down the long hallway.", Hallway1.Get);
        }

        private Return MoveEast()
        {
            return new Return("You enter into the room to your east.", Room6.Get);
        }

        private Return GrabBreastplate()
        {
            CollectItem<BrokenBreastplate>();

            UpdateDescription("A room that was once an armory, there are pieces of armor on many manaquins, with a few other pieces laying about, none of which would fit you. To the south is the hallway. To the east is a room.");

            UnregisterHandler(new Command("grab", "breastplate"));
            UnregisterHandler(new Command("grab", "breastplate", "broken"));

            return new Return("You pick up the broken breastplate.");
        }

        public override void Respawn()
        {
            AddEnemy(() => new Slime("Slimey"));
        }
    }
}
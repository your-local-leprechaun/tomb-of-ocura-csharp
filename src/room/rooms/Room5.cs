using Commands;
using Enemies;
using Items;
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

            // Add OG Items
            // AddItem(new BasicKey());

            // Add Enemies
            AddEnemy(new Slime("Slimey"));
        }

        // Custom Choice Methods
        private Return MoveSouth()
        {
            return new Return("You walk down the long hallway.", Hallway1.Get);
        }
    }
}
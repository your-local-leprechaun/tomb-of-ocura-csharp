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
            // RegisterHandler(new Command("grab", "key"), GrabKey);

            // Add OG Items
            // AddItem(new BasicKey());

            // Add Enemies
            AddEnemy(new Slime("Slimey"));
            AddEnemy(new Slime("Slimer", 16));
        }

        // Custom Choice Methods
        // private Return GrabKey()
        // {
        //     UpdateDescription("You stand in a jail cell. There is a cell door in front of you and a bed behind you.");

        //     CollectItem<BasicKey>();
        //     UnregisterHandler(new Command("grab", "key"));

        //     return new Return("You pickup the key");
        // }
    }
}
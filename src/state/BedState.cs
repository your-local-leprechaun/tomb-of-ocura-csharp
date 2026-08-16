
using Commands;
using Parser;
using Returns;
using Rooms;
using State;

public class BedState : IState
{
    public string Name => "Rest";

    public Return Execute(Command command)
    {
        if (command == new Command("leave", "bed"))
        {
            return new Return("You get out of bed, ready for the challenge.", previous: true);
        }
        throw new UnknownCommandException(command);
    }

    public Return Activate()
    {
        // Heal Player
        Player.Get.CurrHealth = Player.Get.MaxHealth;

        // Reset all rooms!
        RoomRegistry.RespawnTouched();

        return new Return("As you lay down, your health is fully restored and the tomb comes back to life with monsters...");
    }
}
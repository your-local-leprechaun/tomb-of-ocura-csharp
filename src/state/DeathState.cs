
using Commands;
using Returns;
using Rooms;
using State;

public class DeathState : IState
{
    public string Name => "";
    public Return Execute(Command command)
    {
        return new Return("Death State Execute");
    }

    public Return Activate()
    {
        // Heal Player
        Player.Get.CurrHealth = Player.Get.MaxHealth;

        // Remove all Experience
        Player.Get.PlayerExperience = 0;

        // Reset all rooms!
        RoomRegistry.RespawnTouched();

        // Set state to checkpoint in RoomRegistry
        return new Return("You awake at your last checkpoint.", RoomRegistry.Checkpoint as IState);
    }
}
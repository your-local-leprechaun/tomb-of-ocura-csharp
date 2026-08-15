
using Commands;
using Parser;
using Returns;
using State;

public class BedState : IState
{
    public Return Execute(Command command)
    {
        if (command == new Command("leave", "bed"))
        {
            return new Return("You get out of bed, ready for the challenge.", previous: true);
        }
        throw new CommandException("--Unknown Error--");
    }

    public Return Activate()
    {
        // Heal Player
        Player.Get.CurrHealth = Player.Get.MaxHealth;

        // Reset all rooms!

        return new Return("As you lay down, your health is fully restored!");
    }
}
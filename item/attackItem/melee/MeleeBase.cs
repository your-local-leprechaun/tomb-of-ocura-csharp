using TombOfOcura.Item.AttackItem;


namespace TombOfOcura.Item.AttackItem.Melee
{
    public abstract class Melee : AttackItem, IMelee
    {
        protected Melee(string name, string description, string type) : base(name, description, type) {}

        /*
        Additional melee-specific methods to make Attack function work properly.

        Examples:
        Hit() - calculates if the attack hits the target based on accuracy and target's stats
        Damage() - calculates damage dealt based on weapon stats and target's defense
        Crit() - determines if the attack is critical or not
        Hold() - manages the hold action, where holding for a turn ensures next attack hits.
        */
    }
}
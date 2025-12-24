
namespace TombOfOcura.Item.AttackItem.Spell
{
    public abstract class Spell : AttackItem, ISpell
    {
        protected Spell(string name, string description, string type) : base(name, description, type) {}

        /*
        Impliment additional spell-specific methods to make attack function properly.
        These ones should be private instead of public, so more like a helper backend method.

        Examples:
        ManaCost() - calculates and deducts the mana cost
        SpellEffect() - applies the spell's effect on the target
        */
    }
}
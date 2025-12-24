
namespace TombOfOcura.Item.AttackItem.Melee
{
    public interface IMelee : IAttackItem
    {
        /*
        Mainly just used for a marker interface for melee attack items, but the base class
        is very important in this case for the helper functions they will all use. This
        could evolve more if I change how damage values are assigned to melee weapons.
        */
    }
}
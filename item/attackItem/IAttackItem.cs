
namespace TombOfOcura.Item.AttackItem
{
    public interface IAttackItem : IItem
    {
        /*
        The method called whenever the item is used to attack. Changes based on
        type of attack, melee or spell, but also could have other effects based on previous
        moves such as Hold action
        */
        void Attack();
    }
}
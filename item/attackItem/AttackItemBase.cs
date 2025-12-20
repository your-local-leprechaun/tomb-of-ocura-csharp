
namespace TombOfOcura.Item.AttackItem
{
    public abstract class AttackItem : Item, IAttackItem
    {
        public abstract void Attack();
        protected AttackItem(string name, string description, string type) : base(name, description, type) { }
    }
}
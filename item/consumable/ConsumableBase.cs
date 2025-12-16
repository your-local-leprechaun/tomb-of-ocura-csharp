
namespace TombOfOcura.Item.Consumable
{
    public abstract class Consumable : Item, IConsumable
    {
        public abstract void Consume();
        protected Consumable(string name, string description, string type) : base(name, description, type) { }
    }
}
using Returns;

namespace Items.Consumables
{
    public interface IConsumable
    {
        Return Consume();
    }

    public class ConsumableBase : ItemBase, IConsumable
    {
        public ConsumableBase(string name, string description) : base (name, description, ItemType.Consumable)
        {
            
        }

        public virtual Return Consume()
        {
            Inventory.Get.RemoveItem(this);
            return new Return("");
        }
    }
}
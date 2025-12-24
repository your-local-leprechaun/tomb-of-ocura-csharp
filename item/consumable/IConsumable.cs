
namespace TombOfOcura.Item.Consumable
{
    public interface IConsumable : IItem
    {
        /*
        This should eventually have a target that default to the player, but this method
        represents consuming the item, however that might happen. For example, downing a health potion,
        reading a note, and using a scroll would all be different implementations of a consume method.

        Consume is just for any that removes that item from the inventory after used.
        */
        void Consume();
    }
}
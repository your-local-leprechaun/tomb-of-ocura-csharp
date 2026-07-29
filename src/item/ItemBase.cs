
namespace Items
{
    public enum ItemType
    {
        Misc,
        Equipment,
        Consumable    
    }

    public class ItemBase : IItem
    {
        public string Description { get; init; }= "";
        public ItemType Type {get; init; } = ItemType.Misc;
        public string ItemName { get; init; } = "";

        public ItemBase (string description, ItemType type, string itemName)
        {
            Description = description;
            Type = type;
            ItemName = itemName;
        }
    }
}
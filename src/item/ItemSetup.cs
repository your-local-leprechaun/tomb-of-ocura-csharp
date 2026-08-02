namespace Items
{
    public enum ItemType
    {
        Misc,
        Equipment,
        Consumable
    }

    public interface IItem
    {
        string Description { get; }
        ItemType Type { get; }
        string ItemName { get; }
        public string GetInfo();
    }

    public class ItemBase : IItem
    {
        public string Description { get; init; } = "";
        public ItemType Type { get; init; } = ItemType.Misc;
        public string ItemName { get; init; } = "";

        public virtual string GetInfo()
        {
            return $"{ItemName}\n{Description}";
        }

        public ItemBase(string itemName, string description, ItemType type)
        {
            Description = description;
            Type = type;
            ItemName = itemName;
        }
    }
}
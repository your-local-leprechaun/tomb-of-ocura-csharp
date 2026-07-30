
namespace Items
{
    public class BasicKey : IItem
    {
        public string Description { get; init; }
        public ItemType Type { get; init; }
        public string ItemName { get; init; }

        public BasicKey()
        {
            Description = "A basic key.";
            Type = ItemType.Misc;
            ItemName = "Basic Key";
        }
    }    
}


namespace Items
{
    public interface IItem
    {
        string Description { get; }
        ItemType Type { get; }
        string ItemName { get; }
    }    
}
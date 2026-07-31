
namespace Items
{
    public class RedKey : IItem
    {
        public string Description { get; init; }
        public ItemType Type { get; init; }
        public string ItemName { get; init; }

        public RedKey()
        {
            Description = "A bright red key.";
            Type = ItemType.Misc;
            ItemName = "Red Key";

            Parser.Parser.RegisterNoun("Key");
            Parser.Parser.RegisterAdjective("Red");
        }
    }    
}
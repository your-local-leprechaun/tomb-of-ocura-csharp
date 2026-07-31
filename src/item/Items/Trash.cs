
namespace Items
{
    public class Trash : IItem
    {
        public string Description { get; init; }
        public ItemType Type { get; init; }
        public string ItemName { get; init; }

        public Trash()
        {
            Description = "A small clump of trash you found on the floor.";
            Type = ItemType.Misc;
            ItemName = "Trash";

            Parser.Parser.RegisterNoun("Trash");
        }
    }    
}
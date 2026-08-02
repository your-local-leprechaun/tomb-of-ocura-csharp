
namespace Items
{
    public class Trash : ItemBase
    {
        public Trash() : base(
            "Trash",
            "A small clump of trash you found on the floor.",
            ItemType.Misc
        )
        {
            Parser.Parser.RegisterNoun("Trash");
        }
    }    
}
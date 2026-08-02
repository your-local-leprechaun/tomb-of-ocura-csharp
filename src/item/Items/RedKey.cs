
namespace Items
{
    public class RedKey : ItemBase
    {

        public RedKey() : base(
            "Red Key",
            "A bright red key.",
            ItemType.Misc
        )
        {
            Parser.Parser.RegisterNoun("Key");
            Parser.Parser.RegisterAdjective("Red");
        }
    }    
}
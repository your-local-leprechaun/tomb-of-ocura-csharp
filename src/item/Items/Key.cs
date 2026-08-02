
namespace Items
{
    public class BasicKey : ItemBase
    {
        public BasicKey() : base(
            "Basic Key",
            "A basic key. It's nothing special.",
            ItemType.Misc
        )
        {
            Parser.Parser.RegisterNoun("Key");
            Parser.Parser.RegisterAdjective("Basic");
        }
    }    
}
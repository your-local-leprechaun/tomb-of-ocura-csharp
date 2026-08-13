
namespace Items.Equipment
{
    public class BasicSword : MeleeBase
    {
        public BasicSword() : base (
            "Basic Sword",
            "This sword doesn't look like anything special, but at least it's not rusty.",
            80,
            3,
            6
        )
        {
            Parser.Parser.RegisterAdjective("Basic");
            Parser.Parser.RegisterNoun("Sword");
        }
    }
}

using Status;

namespace Items.Equipment
{
    public class PoisonMist : SpellBase
    {
        public PoisonMist() : base (
            () => new Poison(3, 1),
            "Poison Mist",
            "A spell that creates a small cloud of poison mist that can be directed towards a single target."
        )
        {
            Parser.Parser.RegisterAdjective("Poison");
            Parser.Parser.RegisterNoun("Mist");
        }
    }
}
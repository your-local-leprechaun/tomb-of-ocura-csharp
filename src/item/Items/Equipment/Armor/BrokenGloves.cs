namespace Items.Equipment
{
    public class BrokenGloves : ArmorBase
    {
        public BrokenGloves() : base (
            "Broken Gloves",
            "A pair of gloves that used to be fine leather, but is more broken now.",
            EquipType.Gloves,
            1
        )
        {
            Parser.Parser.RegisterAdjective("Broken");
            Parser.Parser.RegisterNoun("Gloves");
        }
    }
}
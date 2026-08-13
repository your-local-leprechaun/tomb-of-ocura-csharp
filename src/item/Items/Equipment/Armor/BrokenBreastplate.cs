namespace Items.Equipment
{
    public class BrokenBreastplate : ArmorBase
    {
        public BrokenBreastplate() : base (
            "Broken Breastplate",
            "A chest piece that was once fine leather but is now tattered and falling apart. It should still help.",
            EquipType.Chest,
            1
        )
        {
            Parser.Parser.RegisterAdjective("Broken");
            Parser.Parser.RegisterNoun("Breastplate");
        }
    }
}
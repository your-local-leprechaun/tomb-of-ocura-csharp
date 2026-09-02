namespace Items.Equipment
{
    public class RustySword : MeleeBase
    {
        public RustySword() : base (
            "Rusty Sword",
            "An old, rusty sword. It doesn't look powerful.",
            75,
            4,
            6
        )
        {
            Parser.Parser.RegisterAdjective("Rusty");
            Parser.Parser.RegisterNoun("Sword");
        }
    }
}

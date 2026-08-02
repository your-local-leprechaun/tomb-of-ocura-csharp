
namespace Items.Equipment
{
    public class RustySword : EquipmentBase, IMelee
    {
        public RustySword() : base (
            "An old, rusty sword. It doesn't look powerful.",
            "Rusty Sword",
            EquipType.Melee
        )
        {
            Parser.Parser.RegisterNoun("Sword");
            Parser.Parser.RegisterAdjective("Rusty");
        }

        public bool Swing()
        {
            return true;
        }

        public int Damage()
        {
            return 5;
        }
    }
}


namespace TombOfOcura.Item.AttackItem.Spell
{
    public class SpellExample : Spell
    {
        public SpellExample() : base("Example Spell", "A spell used for testing purposes.", "Spell") {}

        public override void Attack()
        {
            // Implement spell-specific attack logic
            Console.WriteLine("Casting Example Spell!");
        }
    }
}
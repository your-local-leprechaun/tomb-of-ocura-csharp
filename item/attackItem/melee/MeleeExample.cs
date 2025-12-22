

namespace TombOfOcura.Item.AttackItem.Melee
{
    public class MeleeExample : Melee
    {
        public MeleeExample() : base("Sword of Testing", "A short sword used for testing.", "Melee") {}

        public override void Attack()
        {
            Console.WriteLine("Attack with Sword of Testing!");
        }
    }
}
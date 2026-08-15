using Returns;

namespace Items.Consumables
{
    public class HealthPotion : ConsumableBase
    {
        public HealthPotion() : base(
            "Health Potion",
            "A small health potion. It will heal a small ammount of health when consumed."
        )
        {
            Parser.Parser.RegisterAdjective("Health");
            Parser.Parser.RegisterNoun("Potion");
        }

        public override Return Consume()
        {
            // What does the potion actually do? Heals random amount between two numbers
            int heals = new Range(3, 9).Roll(Random.Shared);
            int trueHeal = Player.Get.Heal(heals);

            // Remove from inventory/do shared actions.
            base.Consume();
            return new Return($"You gain {trueHeal} HP!");
        }
    }
}
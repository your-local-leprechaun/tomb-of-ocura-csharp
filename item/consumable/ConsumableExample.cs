
namespace TombOfOcura.Item.Consumable
{
    public class ConsumableExample : Consumable
    {
        public ConsumableExample() : base("Health Potion", "Restores 10 HP when consumed.", "Misc") { }

        public override void Consume()
        {
            Console.WriteLine("You drink the Health Potion and restore 10 HP.");
        }
    }
}
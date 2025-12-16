
namespace TombOfOcura.Item.Armor
{
    public abstract class Armor : Item, IArmor
    {
        public int Defense { get; }
        public int RequiredLevel { get; }
        protected Armor(string name, string description, int defense, int requiredLevel, string type = "Armor") : base(name, description, type)
        {
            Defense = defense;
            RequiredLevel = requiredLevel;
        }
    }
}
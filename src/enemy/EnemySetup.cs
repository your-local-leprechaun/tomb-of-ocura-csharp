using Combatants;

namespace Enemies
{
    public interface IEnemy
    {
        string Description { get; }
        int Level { get; }
    }

    public class EnemyBase : CombatantBase, IEnemy
    {
        public string Description { get; init; }
        public int Level { get; init; } = 1;

        public EnemyBase(string name, int maxHealth, string description, int level) : base(
            name,
            maxHealth
        )
        {
            Level = level;
            Description = description;
        }

        public string Status()
        {
            return $"{Name}\n{CurrHealth}/{MaxHealth}\n{Description}";
        }
    }
}
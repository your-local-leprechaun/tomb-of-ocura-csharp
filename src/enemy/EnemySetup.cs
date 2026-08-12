using Combatants;
using Commands;
using Returns;
using Stats;

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

        public EnemyBase(
            string name,
            int maxHealth,
            string description,
            int level,
            IDictionary<StatType, int> initialStats
        ) : base(name, maxHealth, initialStats)
        {
            Level = level;
            Description = description;
            Parser.Parser.RegisterNoun(name);
        }

        public string Status()
        {
            return $"{Name}\n{Level}\n{CurrHealth}/{MaxHealth}\n{Description}";
        }

        public override Return TakeAction()
        {
            return new Return($"{Name} takes their turn");
        }
    }
}
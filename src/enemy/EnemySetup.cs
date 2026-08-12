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

    /// <summary>
    /// One option an enemy can pick on its turn: swing with the given
    /// odds/damage range and a weight controlling how often it's chosen.
    /// </summary>
    public record EnemyMove(string Name, int Weight, int HitChance, int MinDamage, int MaxDamage);

    public class EnemyBase : CombatantBase, IEnemy
    {
        public string Description { get; init; }
        public int Level { get; init; } = 1;

        // The moves an enemy can choose between on its turn. Override per-enemy.
        protected virtual List<EnemyMove> Moves { get; } = new();

        // How often the enemy chooses to hold instead of attacking, relative to
        // the combined weight of its Moves. Not offered while already holding.
        protected virtual int HoldWeight => 200;

        public EnemyBase(
            string name,
            int maxHealth,
            string description,
            int level,
            int experience,
            IDictionary<StatType, int> initialStats
        ) : base(name, maxHealth, experience, initialStats)
        {
            Level = level;
            Description = description;
            Parser.Parser.RegisterNoun(name);
        }

        public string Status()
        {
            return $"{Name}\n{Level}\n{CurrHealth}/{MaxHealth}\n{Description}";
        }

        protected bool TryHit(int hitChance)
        {
            int roll = Random.Shared.Next(0, 101);
            return roll <= hitChance;
        }

        protected int CalcDamage(int min, int max)
        {
            return Random.Shared.Next(min, max + 1);
        }

        protected Return PerformAttack(EnemyMove move)
        {
            if (!(TryHit(move.HitChance) || _hold))
            {
                return new Return($"{Name} attacks with {move.Name} but misses!");
            }
            _hold = false;

            int damage = CalcDamage(move.MinDamage, move.MaxDamage);
            int dealtDamage = Player.Get.Damage(damage);

            return new Return($"{Name} hits Player with {move.Name} for {dealtDamage}!\nPlayer HP ({Player.Get.CurrHealth}/{Player.Get.MaxHealth})");
        }

        protected Return PerformHold()
        {
            Hold();
            return new Return($"{Name} braces, ensuring its next attack will land!");
        }

        // Not wired into ChooseAndAct yet - there's no notion of "allies" on the
        // combatants list to pick a target from. Shape is here for when that lands.
        protected Return PerformHeal(ICombatant ally, int healAmount)
        {
            ally.Heal(healAmount);
            return new Return($"{Name} heals {ally.Name} for {healAmount}!");
        }

        protected virtual Return ChooseAndAct(List<ICombatant> combatants)
        {
            List<(int Weight, Func<Return> Action)> pool = new();

            foreach (EnemyMove move in Moves)
            {
                pool.Add((move.Weight, () => PerformAttack(move)));
            }

            if (!_hold)
            {
                pool.Add((HoldWeight, PerformHold));
            }

            return WeightedPick(pool)();
        }

        private static Func<Return> WeightedPick(List<(int Weight, Func<Return> Action)> pool)
        {
            int total = pool.Sum(entry => entry.Weight);
            int roll = Random.Shared.Next(total);

            int cumulative = 0;
            foreach (var (weight, action) in pool)
            {
                cumulative += weight;
                if (roll < cumulative)
                {
                    return action;
                }
            }

            return pool[^1].Action;
        }

        public override Return TakeAction(List<ICombatant> combatants)
        {
            return ChooseAndAct(combatants);
        }
    }
}
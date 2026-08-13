using Combatants;
using Commands;
using Returns;
using Stats;

namespace Enemies
{
    public class Slime : EnemyBase, ICombatant
    {
        public Slime(string name, int test = 4) : base(
            name,
            new Range(2, 5).Roll(Random.Shared),
            "A green-ish gelatinous...thing. You're not sure what it's made of.",
            1,
            50,
            new Dictionary<StatType, int>
            {
                { StatType.Might, 4},
                { StatType.Arcana, 4},
                { StatType.Fortitude, 4},
                { StatType.Vitality, test},
                { StatType.Chance, 4},
            }
        )
        { }

        protected override List<EnemyMove> Moves { get; } = new()
        {
            new EnemyMove("Ooze", 100, 65, 1, 3),
        };
    }
}
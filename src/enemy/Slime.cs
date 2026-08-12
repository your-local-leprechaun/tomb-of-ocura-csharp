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
            "Description Here",
            1,
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

        public override Return TakeAction()
        {
            return base.TakeAction();
        }
    }
}
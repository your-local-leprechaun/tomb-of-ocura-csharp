using Combatants;
using Commands;
using Returns;

namespace Enemies
{
    public class Slime : EnemyBase, ICombatant
    {
        public Slime(string name) : base (
            name,
            new Range(2, 5).Roll(Random.Shared),
            "Description Here",
            1
        ) {}

        public override Return TakeAction(Command? command = null)
        {
            return base.TakeAction(command);
        }
    }
}
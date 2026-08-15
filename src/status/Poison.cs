using Combatants;
using Returns;

namespace Status
{
    public class Poison : StatusBase
    {
        public int Damage = 1;
        public Poison(int time, int damage) : base(time, "poisoned")
        {
            Damage = damage;
        }

        public override Return Inflict(ICombatant target)
        {
            target.DirectDamage(Damage);
            Time -= 1;

            return new Return($"{target.Name} takes {Damage} damage from poison!\n{target.Name} HP({target.CurrHealth}/{target.MaxHealth})");
        }
    }
}
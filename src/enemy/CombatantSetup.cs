
using Commands;
using Returns;

namespace Combatants
{
    public interface ICombatant
    {
        string Name { get; }
        int MaxHealth { get; }
        int CurrHealth { get; }
        void Damage(int damage);
        void Heal(int damage);
        //StatusManager in the future

        Return TakeAction(Command? command = null);
    }

    public class CombatantBase
    {
        public CombatantBase(string name, int maxHealth)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrHealth = MaxHealth;
        }

        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int CurrHealth { get; set; }

        public void Damage(int damage)
        {
            CurrHealth -= damage;
        }

        public void Heal(int healAmmount)
        {
            CurrHealth += healAmmount;
        }

        public virtual Return TakeAction(Command? command = null)
        {
            return new Return($"{nameof(CombatantBase)}'s turn");
        }
    }   
}
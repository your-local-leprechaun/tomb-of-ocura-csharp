
using Commands;
using Returns;
using Stats;

namespace Combatants
{
    public interface ICombatant
    {
        string Name { get; }
        int MaxHealth { get; }
        int CurrHealth { get; }
        StatManager Stats { get; }
        EquipmentManager Equipment { get; }
        void Damage(int damage);
        void Heal(int damage);

        //StatusManager in the future

        Return TakeAction();
    }

    public class CombatantBase
    {
        public CombatantBase(string name, int maxHealth, IDictionary<StatType, int>? initialStats = null)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrHealth = MaxHealth;
            Stats = new StatManager(initialStats);
            Equipment = new EquipmentManager();
        }

        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int CurrHealth { get; set; }

        public StatManager Stats { get; }
        public EquipmentManager Equipment { get; }

        public void Damage(int damage)
        {
            CurrHealth -= damage;
        }

        public void Heal(int healAmmount)
        {
            CurrHealth += healAmmount;
        }

        public virtual Return TakeAction()
        {
            return new Return($"{Name}'s turn");
        }
    }   
}
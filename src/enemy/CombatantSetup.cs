
using Commands;
using Items.Equipment;
using Returns;
using Stats;

namespace Combatants
{
    public interface ICombatant
    {
        string Name { get; }
        int MaxHealth { get; }
        int CurrHealth { get; }
        int Experience { get; }
        StatManager Stats { get; }
        EquipmentManager Equipment { get; }

        StatusManager Conditions { get; }

        int Damage(int damage);
        void DirectDamage(int damage);
        void Heal(int damage);

        Return TakeAction(List<ICombatant> combatants);
    }

    public class CombatantBase
    {
        public CombatantBase(string name, int maxHealth, int experience, IDictionary<StatType, int>? initialStats = null, IDictionary<EquipType, IEquipment>? initialEquipment = null)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrHealth = MaxHealth;
            Experience = experience;
            Stats = new StatManager(initialStats);
            Equipment = new EquipmentManager(initialEquipment);
            Conditions = new StatusManager();
        }

        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int CurrHealth { get; set; }
        public int Experience { get; set; }
        protected bool _hold = false;

        public StatManager Stats { get; }
        public EquipmentManager Equipment { get; }
        public StatusManager Conditions { get; }

        public int Damage(int damage)
        {
            int dealtDamage = damage - Equipment.TotalReduction();
            if (dealtDamage < 0)
            {
                dealtDamage = 0;
            }
            CurrHealth -= dealtDamage;
            return dealtDamage;
        }

        public void DirectDamage(int damage)
        {
            CurrHealth -= damage;
        }

        public void Heal(int healAmmount)
        {
            CurrHealth += healAmmount;
        }

        protected void Hold()
        {
            if (_hold == true)
            {
                throw new Exception("-Already Holding-");
            }
            _hold = true;
        }

        public virtual Return TakeAction(List<ICombatant> combatants)
        {
            return new Return($"{Name}'s turn");
        }
    }   
}
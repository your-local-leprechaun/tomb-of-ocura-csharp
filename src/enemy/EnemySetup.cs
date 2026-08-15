using System.Runtime;
using Combatants;
using Commands;
using Items;
using Items.Equipment;
using Returns;
using Stats;

namespace Enemies
{
    public interface IEnemy : ICombatant
    {
        string Description { get; }
        int Level { get; }
        string Status();
        List<IItem> RollDrops();
    }

    /// <summary>
    /// One option an enemy can pick on its turn: swing with the given
    /// odds/damage range and a weight controlling how often it's chosen.
    /// </summary>
    public record EnemyMove(string Name, int Weight, int HitChance, int MinDamage, int MaxDamage);

    /// <summary>
    /// One possible item drop: Factory builds a fresh item instance, Chance is an
    /// independent 0-100 percent roll (not a weight - every entry is rolled separately).
    /// </summary>
    public record ItemDrop(Func<IItem> Factory, int Chance);

    public class EnemyBase : CombatantBase, IEnemy, ICombatant
    {
        public string Description { get; init; }
        public int Level { get; init; } = 1;

        // The moves an enemy can choose between on its turn. Override per-enemy.
        protected virtual List<EnemyMove> Moves { get; } = new();

        // How often the enemy chooses to hold instead of attacking, relative to
        // the combined weight of its Moves. Not offered while already holding.
        protected virtual int HoldWeight => 15;

        // Weight for swinging with the equipped Melee item (or fists, if none equipped)
        // instead of a scripted Moves entry. 0 disables it. Set on a per-enemy basis.
        protected virtual int WeaponAttackWeight => 0;

        // Items this enemy can drop on death. Null if it drops nothing. Each entry is
        // rolled independently against its own Chance, so multiple items can drop at once.
        protected virtual List<ItemDrop>? Drops => null;

        public EnemyBase(
            string name,
            int maxHealth,
            string description,
            int level,
            int experience,
            IDictionary<StatType, int> initialStats,
            IDictionary<EquipType, IEquipment>? initialEquipment = null
        ) : base(name, maxHealth, experience, initialStats, initialEquipment)
        {
            Level = level;
            Description = description;
            Parser.Parser.RegisterNoun(name);
        }

        public List<IItem> RollDrops()
        {
            List<IItem> dropped = new();
            if (Drops is null)
            {
                return dropped;
            }

            foreach (ItemDrop drop in Drops)
            {
                if (Random.Shared.Next(0, 101) <= drop.Chance)
                {
                    dropped.Add(drop.Factory());
                }
            }

            return dropped;
        }

        public string Status()
        {
            string status = $"{Name}\nLvl {Level} {GetType().Name}\nHealth: {CurrHealth}/{MaxHealth}";

            var equipped = Equipment.EquippedItems.Values.Where(item => item != null);
            if (equipped.Any())
            {
                var equipmentEntries = equipped.Select(item => $"{item!.ItemName} ({item.EquipSlot})");
                status += "\nEquipment: " + string.Join(", ", equipmentEntries);
            }

            status += $"\n{Description}";

            return status;
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
            if (dealtDamage != 0)
            {
                return new Return($"{Name} hits Player with {move.Name} for {dealtDamage}!\nPlayer HP ({Player.Get.CurrHealth}/{Player.Get.MaxHealth})");
            }
            return new Return($"{Name} hits Player with {move.Name}, but deals no damage!");
        }

        protected Return PerformWeaponAttack()
        {
            IEquipment? equipped = Equipment.EquippedSlot(EquipType.Melee);
            IMelee weapon = equipped as IMelee ?? new Fist();

            if (!(weapon.TryHit() || _hold))
            {
                return new Return($"{Name} attacks with {weapon.ItemName} but misses!");
            }
            _hold = false;

            int damage = weapon.CalcDamage();
            int dealtDamage = Player.Get.Damage(damage);
            if (dealtDamage != 0)
            {
                return new Return($"{Name} hits Player with {weapon.ItemName} for {dealtDamage}!\nPlayer HP ({Player.Get.CurrHealth}/{Player.Get.MaxHealth})");
            }
            return new Return($"{Name} hits Player with {weapon.ItemName}, but deals no damage!");
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

            if (WeaponAttackWeight > 0)
            {
                pool.Add((WeaponAttackWeight, PerformWeaponAttack));
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
            List<string> ReturnMessage = [];

            // If statuses are around, Inflict all!
            Return response = Conditions.Aflict(this);
            if (response.Message != "")
            {
                ReturnMessage.Add(response.Message);
            }
            
            // If enemy is dead, return
            if (CurrHealth <= 0)
            {
                ReturnMessage.Add($"{Name} died! {Experience}xp gained!");
                Player.Get.PlayerExperience += Experience;
                return new Return(string.Join("\n", ReturnMessage));    
            }

            // Take turn if not dead!
            ReturnMessage.Add(ChooseAndAct(combatants).Message);

            return new Return(string.Join("\n", ReturnMessage));
        }
    }
}
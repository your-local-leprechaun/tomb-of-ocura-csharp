
using Stats;

namespace Items.Equipment
{
    public interface IMelee : IEquipment
    {
        bool TryHit();
        int CalcDamage();
    }

    public class MeleeBase : EquipmentBase, IMelee
    {
        int MinDamage;
        int MaxDamage;
        int HitChance;

        public MeleeBase(string itemName, string description, int hitChance, int minDamage, int maxDamage, Tuple<StatType, int>? requirement = null) : base(
            description,
            itemName,
            EquipType.Melee,
            requirement
        )
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            HitChance = hitChance;
        }

        public bool TryHit()
        {
            int randomHit = Random.Shared.Next(0, 101);
            if (randomHit > HitChance)
            {
                // Miss
                return false;
            }
            return true;
        }

        public int CalcDamage()
        {
            int random = Random.Shared.Next(MinDamage, MaxDamage + 1);
            return random;
        }

        public override string GetInfo()
        {
            return $"{ItemName}{(IsEquipped ? " *" : "")}\n{Description}\nHit Chance: {HitChance}%\nDamage: {MinDamage}-{MaxDamage}";
        }

    }
}

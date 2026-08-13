

using Stats;

namespace Items.Equipment
{
    public interface IArmor : IEquipment
    {
        int Reduction { get; }
    }

    public class ArmorBase : EquipmentBase, IArmor
    {
        public int Reduction { get; init; }

        public ArmorBase(
            string itemName,
            string description,
            EquipType type,
            int reduction,
            Tuple<StatType, int>? requirement = null) : base(
                description,
                itemName,
                type,
                requirement
            )
        {
            Reduction = reduction;
        }

        public override string GetInfo()
        {
            return $"{ItemName}{(IsEquipped ? "*" : "")}\n{Description}\nArmor Type: {EquipSlot}\nDamage Reduction: {Reduction}";
        }
    }
}
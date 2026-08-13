using Returns;
using Stats;

namespace Items.Equipment
{
    public enum EquipType
    {
        Head,
        Chest,
        Gloves,
        Legs,
        Melee,
        Magic,
        Ring
    }
    public interface IEquipment : IItem
    {
        void Equip();
        void Unequip();
        EquipType EquipSlot { get; }
        bool IsEquipped { get; }
    }

    public class EquipmentBase : ItemBase, IEquipment
    {
        public Tuple<StatType, int>? Requirement = null;
        public EquipType EquipSlot { get; init; } = EquipType.Head;
        public bool IsEquipped { get; set; } = false;

        public override string GetInfo()
        {
            return $"{ItemName}{(IsEquipped ? "*" : "")}\nType: {EquipSlot}\n{Description}";
        }

        public void Equip()
        {
            // Check requirements here first!
            // call to Player.Equipment to equip this item. It'll run through the rest
            Player.Get.Equipment.EquipItem(EquipSlot, this);
            IsEquipped = true;
        }

        public void Unequip()
        {
            // Call to Player.Equipment to unequip this item.
            Player.Get.Equipment.UnequipItem(EquipSlot, this);
            IsEquipped = false;
        }

        public EquipmentBase(string description, string itemName, EquipType type, Tuple<StatType, int>? requirement = null) : base(
            itemName,
            description,
            ItemType.Equipment
        )
        {
            EquipSlot = type;
            if (requirement != null)
            {
                Requirement = requirement;
            }
        }
    }
}

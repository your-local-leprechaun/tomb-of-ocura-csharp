using Items.Equipment;

public class EquipmentManager
{
    public readonly Dictionary<EquipType, IEquipment?> EquippedItems;

    public EquipmentManager(IDictionary<EquipType, IEquipment>? initial = null)
    {
        EquippedItems = Enum.GetValues<EquipType>()
            .ToDictionary(slot => slot, slot => initial != null && initial.TryGetValue(slot, out var equipped) ? equipped : null);
    }

    public void EquipItem(EquipType type, IEquipment equipment)
    {
        EquippedItems[type]?.Unequip();
        EquippedItems[type] = equipment;
    }

    public void UnequipItem(EquipType type, IEquipment equipment)
    {
        if (EquippedItems.TryGetValue(type, out var equipped) && equipped == equipment)
        {
            EquippedItems[type] = null;
        }
    }

    public string Status()
    {
        var lines = EquippedItems.Select(slot =>
            $"  {slot.Key}: {(slot.Value is null ? "(empty)" : slot.Value.ItemName)}");

        return "Equipment:\n" + string.Join('\n', lines) + "\n";
    }

    public bool Equipped(IEquipment item, EquipType slot)
    {
        return EquippedItems[slot]?.GetType() == item.GetType();
    }

    public IEquipment? EquippedSlot(EquipType slot)
    {
        return EquippedItems[slot];
    }

    public int TotalReduction()
    {
        int total = 0;
        foreach (var item in EquippedItems.Values)
        {
            if (item is IArmor armor)
            {
                total += armor.Reduction;
            }
        }
        return total;
    }
}
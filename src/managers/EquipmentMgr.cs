using Items.Equipment;

public class EquipmentMgr
{
    private readonly Dictionary<EquipType, IEquipment?> _equippedItems;

    public EquipmentMgr(Dictionary<EquipType, IEquipment?>? initial = null)
    {
        _equippedItems = Enum.GetValues<EquipType>()
            .ToDictionary(slot => slot, slot => initial?.GetValueOrDefault(slot));
    }

    public void EquipItem(EquipType type, IEquipment equipment)
    {
        _equippedItems[type]?.Unequip();
        _equippedItems[type] = equipment;
    }

    public void UnequipItem(EquipType type, IEquipment equipment)
    {
        if (_equippedItems.TryGetValue(type, out var equipped) && equipped == equipment)
        {
            _equippedItems[type] = null;
        }
    }

    public string Status()
    {
        var lines = _equippedItems.Select(slot =>
            $"  {slot.Key}: {(slot.Value is null ? "(empty)" : slot.Value.ItemName)}");

        return "Equipment:\n" + string.Join('\n', lines) + "\n";
    }

    public bool Equipped(IEquipment item, EquipType slot)
    {
        return _equippedItems[slot]?.GetType() == item.GetType();
    }
}
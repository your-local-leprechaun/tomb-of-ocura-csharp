using Basic;
using Commands;
using Items;
using Items.Equipment;
using Parser;
using Returns;
using State;

public class Inventory : Singleton<Inventory>, IState
{
    // Singleton Stuff
    private Inventory()
    {
        AddItem(new PoisonMist());
    }

    public static Inventory Get => Instance;

    // State Stuff
    public Return Execute(Command command)
    {
        if (command.Verb == "close" && command.Noun == "inventory")
        {
            return new Return("Closing inventory...", previous: true);
        }
        else if (command.Verb == "show" && command.Noun == "inventory")
        {
            return ListItems();
        }
        else if (command.Verb == "check")
        {
            return ExamineItem(command);
        }
        else if (command.Verb == "equip")
        {
            return EquipItem(command);
        }
        else if (command.Verb == "unequip")
        {
            return UnequipItem(command);
        }
        throw new CommandException("--Unknown Command--");
    }

    private Return ExamineItem(Command command)
    {
        // Build the item name
        string searchName = command.Adjective is null ? command.Noun :
        $"{command.Adjective} {command.Noun}";

        IItem? item = _storage.FirstOrDefault(i =>
            string.Equals(i.ItemName, searchName, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            return new Return($"-No {searchName} in your inventory-");
        }

        return new Return(item.GetInfo());
    }

    private Return EquipItem(Command command)
    {
        // Build Item Name
        string searchName = command.Adjective is null ? command.Noun :
        $"{command.Adjective} {command.Noun}";

        IItem? item = _storage.FirstOrDefault(i =>
            string.Equals(i.ItemName, searchName, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            return new Return($"-No {searchName} in your inventory-");
        }
        if (item is not IEquipment equipment)
        {
            return new Return($"-{item.ItemName} is not equippable-");
        }
        IEquipment equip = (IEquipment)item;

        // Equip the item here
        equip.Equip();

        return new Return($"Equipped {item.ItemName}");
    }

    private Return UnequipItem(Command command)
    {
        // Find Item
        string searchName = command.Adjective is null ? command.Noun :
        $"{command.Adjective} {command.Noun}";

        IItem? item = _storage.FirstOrDefault(i =>
            string.Equals(i.ItemName, searchName, StringComparison.OrdinalIgnoreCase));
        
        if (item is null)
        {
            return new Return($"-No {searchName} in your inventory-");
        }
        else if (item is not IEquipment)
        {
            return new Return($"-{searchName} is not equipable-");
        }

        IEquipment equipment = (IEquipment)item;

        // Check if the slot actually has this item
        if (!Player.Get.Equipment.Equipped(equipment, equipment.EquipSlot))
        {
            return new Return($"-{searchName} is not equipped-");
        }

        // If they do, actually unequip it!
        equipment.Unequip();

        return new Return($"Unequipped {item.ItemName}");
    }

    public Return Activate()
    {
        return ListItems();
    }

    private Return ListItems()
    {
        string itemList = "Inventory";

        foreach (IItem item in _storage)
        {
            itemList += $"\n  {item.ItemName}" + (item is IEquipment equip && equip.IsEquipped ? "*" : "");
        }

        return new Return(itemList);
    }

    // Inventory Stuff
    private List<IItem> _storage = new List<IItem>();

    public bool Contains<TItem>() where TItem : IItem
    {
        return _storage.Any(i => i is TItem);
    }

    public void AddItem(IItem item)
    {
        _storage.Add(item);
    }

    public void RemoveItem<TItem>() where TItem : IItem
    {
        var item = _storage.FirstOrDefault(i => i is TItem);
        if (item != null)
        {
            _storage.Remove(item);
        }
    }
}
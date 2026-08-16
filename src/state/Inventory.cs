using Basic;
using Commands;
using Items;
using Items.Consumables;
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
        AddItem(new Trash());
        AddItem(new Trash());
        AddItem(new HealthPotion());
    }

    public static Inventory Get => Instance;

    public string Name => "Inventory";

    // State Stuff
    public Return Execute(Command command)
    {
        string message = "";

        if (command.Verb == "close" && command.Noun == "inventory")
        {
            return new Return("Closing inventory...", previous: true);
        }
        else if (command == new Command("show", "inventory") || command == new Command("show", "items"))
        {
            message = List().Message;
        }
        else if (command.Verb == "check")
        {
            message = Examine(command).Message;
        }
        else if (command.Verb == "equip")
        {
            message = Equip(command).Message;
        }
        else if (command.Verb == "unequip")
        {
            message = Unequip(command).Message;
        }
        else if (command.Verb == "use")
        {
            message = Use(command).Message;
        }
        else
        {
            throw new UnknownCommandException(command);
        }

        // Return actual Return
        return new Return(message, equipment: true);
    }

    public Return Examine(Command command)
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

    public Return Equip(Command command)
    {
        // Build Item Name
        string searchName = command.Adjective is null ? command.Noun :
        $"{command.Adjective} {command.Noun}";

        IItem? item = _storage.FirstOrDefault(i =>
            string.Equals(i.ItemName, searchName, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            throw new InvalidTargetException(searchName);
        }
        if (item is not IEquipment equipment)
        {
            throw new InvalidActionException($"{item.ItemName} is not equippable.");
        }

        // Equip the item here
        equipment.Equip();

        return new Return($"Equipped {item.ItemName}");
    }

    public Return Unequip(Command command)
    {
        // Find Item
        string searchName = command.Adjective is null ? command.Noun :
        $"{command.Adjective} {command.Noun}";

        IItem? item = _storage.FirstOrDefault(i =>
            string.Equals(i.ItemName, searchName, StringComparison.OrdinalIgnoreCase));

        if (item is null)
        {
            throw new InvalidTargetException(searchName);
        }
        else if (item is not IEquipment)
        {
            throw new InvalidActionException($"{searchName} is not equipable.");
        }

        IEquipment equipment = (IEquipment)item;

        // Check if the slot actually has this item
        if (!Player.Get.Equipment.Equipped(equipment, equipment.EquipSlot))
        {
            throw new InvalidActionException($"{searchName} is not equipped.");
        }

        // If they do, actually unequip it!
        equipment.Unequip();

        return new Return($"Unequipped {item.ItemName}");
    }

    public Return Use(Command command)
    {
        // first, check if command.noun is really a consumable item
        // Find Item
        string searchName = command.Adjective is null ? command.Noun :
        $"{command.Adjective} {command.Noun}";

        IConsumable? item = _storage.FirstOrDefault(i =>
            string.Equals(i.ItemName, searchName, StringComparison.OrdinalIgnoreCase)) as IConsumable ?? null;

        if (item is null)
        {
            throw new InvalidTargetException(searchName);
        }

        return item.Consume();
    }

    public Return Activate()
    {
        return new Return(List().Message, equipment: true);
    }

    public Return List()
    {
        string itemList = "Storage:";

        foreach (var group in _storage.GroupBy(i => i.ItemName))
        {
            if (group.First() is IEquipment || group.Count() == 1)
            {
                foreach (IItem item in group)
                {
                    itemList += $"\n {item.ItemName}" + (item is IEquipment equip && equip.IsEquipped ? "*" : "");
                }
            }
            else
            {
                itemList += $"\n {group.Key} x{group.Count()}";
            }
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

    public void RemoveItem(IItem item)
    {
        _storage.Remove(item);
    }
}
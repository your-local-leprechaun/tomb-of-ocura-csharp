using Basic;
using Commands;
using Items;
using Parser;
using Returns;
using State;

public class Inventory : Singleton<Inventory>, IState
{
    // Singleton Stuff
    private Inventory() { }

    public static Inventory Get => Instance;

    // State Stuff
    public Return Execute(Command command)
    {
        if (command.Verb == "close" && command.Noun == "inventory")
        {
            return new Return("Closing inventory...", previous: true);
        }
        else if (command.Verb == "check")
        {
            return ExamineItem(command);
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

        return new Return($"{item.ItemName}\n{item.Description}");
    }

    public Return Activate()
    {
        string itemList = "Inventory";

        foreach (IItem item in _storage)
        {
            itemList += $"\n  {item.ItemName}";
        }

        return new Return(itemList);
    }

    // Inventory Stuff
    private List<IItem> _storage = [];

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
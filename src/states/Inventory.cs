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
        throw new CommandException("--Unknown Command--");
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
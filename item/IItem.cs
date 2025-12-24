
namespace TombOfOcura.Item
{
    public interface IItem
    {
        /*
        Item name
        */
        string Name { get; }
        /*
        Item description, should be about a sentence long.
        */
        string Description { get; }
        /*
        Item type. Currently just a string, but might be better as an enum later.
        */
        string Type { get; }
    }
}
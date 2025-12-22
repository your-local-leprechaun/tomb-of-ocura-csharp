
namespace TombOfOcura.Item
{
    public abstract class Item : IItem
    {
        public string Name { get; }
        public string Description { get; }
        public string Type { get; }
        protected Item(string name, string description, string type = "Misc.")
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }
}
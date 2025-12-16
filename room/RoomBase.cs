
namespace TombOfOcura.Rooms
{
    public abstract class RoomBase : IRoom
    {
        public string Name { get; }
        public string Description { get; protected set;}
        protected readonly List<string> Choices = new();
        protected readonly List<string> Items = new();
        protected readonly List<string> Enemies = new();

        protected RoomBase(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract string HandleCommand(string command);

        public bool RemoveItem(string item)
        {
            return Items.Remove(item);
        }

        public void AddItem(string item)
        {
            Items.Add(item);
            return;
        }

        public List<string> GetChoices()
        {
            return Choices;
        }

        public bool RemoveChoice(string toRemove)
        {
            return Choices.Remove(toRemove);
        }

        public void AddChoice(string toAdd)
        {
            Choices.Add(toAdd);
            return;
        }

        public bool RemoveEnemy(string enemy)
        {
            return Enemies.Remove(enemy);
        }

        public void AddEnemies(string enemy)
        {
            Enemies.Add(enemy);
            return;
        }

        public abstract void RespawnEnemies();
    }
}
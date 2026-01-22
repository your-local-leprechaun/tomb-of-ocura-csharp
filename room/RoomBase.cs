
using TombOfOcura.Enemy;
using TombOfOcura.Item;

namespace TombOfOcura.Rooms
{
    public abstract class Room : IRoom
    {
        public string Name { get; }
        public string Description { get; protected set;}
        protected readonly List<string> Choices = new();
        protected readonly List<IItem> Items = new();
        protected readonly List<IEnemy> Enemies = new();

        protected Room(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public abstract string HandleCommand(string command);

        public bool RemoveItem(IItem item)
        {
            return Items.Remove(item);
        }

        public void AddItem(IItem item)
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

        public bool RemoveEnemy(IEnemy enemy)
        {
            return Enemies.Remove(enemy);
        }

        public void AddEnemies(IEnemy enemy)
        {
            Enemies.Add(enemy);
            return;
        }

        public abstract void RespawnEnemies();


        /*
        Updates the room's description. Protected so only the room can change it's description.
        */
        protected void UpdateDescription(string newDescription)
        {
            Description = newDescription;
            return;
        }
    }
}
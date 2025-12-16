
namespace TombOfOcura.Rooms
{
    /*
    Interface for all Room objects in this text-based rpg adventure game. This script ensures that
    all rooms have a consistent way to interact with them from other classes. 
    */
    interface IRoom
    {
        string Name { get; }
        
        string Description { get; }

        string HandleCommand(string command);

        bool RemoveItem(string item);

        void AddItem(string item);

        List<string> GetChoices();

        bool RemoveChoice(string toRemove);

        void AddChoice(string toAdd);
        
        bool RemoveEnemy(string enemy);

        void AddEnemies(string enemy);

        void RespawnEnemies();
    }
}
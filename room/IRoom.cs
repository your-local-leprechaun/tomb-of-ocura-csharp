
/*
Interface for all Room objects in this text-based rpg adventure game. This script ensures that
all rooms have a consistent way to interact with them from other classes. 
*/
interface IRoom
{
    string Description { get;}

    void RemoveItem();

    void AddItem();

    string[] GetChoices();

    bool RemoveChoice(string toRemove);

    void AddChoice(string toAdd);

    void AddEnemies();

    void RemoveEnemy();

    void RespawnEnemies();
}
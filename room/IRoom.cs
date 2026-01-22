
using TombOfOcura.Enemy;
using TombOfOcura.Item;

namespace TombOfOcura.Rooms
{
    /*
    Interface for all Room objects in this text-based rpg adventure game. This script ensures that
    all rooms have a consistent way to interact with them from other classes. 

    All Rooms should use the singleton pattern, as only one of them should exsist.
    This must be made in each implimentation of the RoomBase class.
    */
    interface IRoom
    {
        /*
        Name of the room, rather than just room + number, this gives it more information, and will
        also be the name actually displayed.

        Example: "Jail Cell", "Symbol Room", "Treasure Chamber"
        */
        string Name { get; }
        
        /*
        Description of the room that will be displayed when entering the room. This should paint the room
        for the player. Will also be changed based on certain actions the player takes.
        */
        string Description { get; }

        /*
        The main public method for this room. This method takes in a command and processes it, returning information
        about what happened as a result in a dictionary. If the command is not valid, it should return an appropriate message.
        If the command results in a change to the room's state, it will be handled within this method, while if we move
        to a new room, that will be passed up and handled by the main game loop.
        */
        string HandleCommand(string command);

        /*
        Removes an item from the room. Returns true if successful, false otherwise.
        */
        bool RemoveItem(IItem item);

        /*
        Adds an item to the room.
        */
        void AddItem(IItem item);

        /*
        Returns the list of choices avaliable in this room that are public. Not all 
        avaliable actions may be listed here, some may be hidden.
        */
        List<string> GetChoices();

        /*
        Removes choice from room's avaliable choices. Returns true if successful, false otherwise.
        */
        bool RemoveChoice(string toRemove);

        /*
        Adds choice to room's avaliable choices.
        */
        void AddChoice(string toAdd);
        
        /*
        Removes an enemy from the room. Returns true if successful, false otherwise.
        */
        bool RemoveEnemy(IEnemy enemy);

        /*
        Adds an enemy to the room. Used by RespawnEnemies to reset the room's enemies back to their original state.
        */
        void AddEnemies(IEnemy enemy);

        /*
        Respawns all enemies in the room back to their original state when a rest is taken.
        */
        void RespawnEnemies();
    }
}
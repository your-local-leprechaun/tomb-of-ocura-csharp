

namespace TombOfOcura.Enemy
{
    /*
    Abstract class for all enemies in this game. Enemies are 
    classifies as any object the player can fight/enter a fighting
    state with. They all have stats, health, and attacking abilities.
    Some have weapons, other have claws, hands, etc. 

    Will be created using an EnemyFactory method instead of directly
    initializing them.

    All enemies implement IStatHolder to manage their stats.

    Example:
        Spiders, slimes, Mad Mage, Skeleton Warrior, etc.
    */
    public interface IEnemy
    {
        string Name { get; }
        int Experience { get; }
    }
}
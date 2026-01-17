

namespace TombOfOcura.Enemy
{
    /*
    Public interface for all enemy spawners in the game. All 
    the spawners do is implement the Spawn() method that just 
    returns a new instance of the enemy type they are responsible
    for. 

    Example:
        SlimeSpawner, SpiderSpawner, etc.
    */
    public interface ISpawner<T>
    {
        T Spawn();
    }
}
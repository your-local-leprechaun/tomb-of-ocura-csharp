

namespace TombOfOcura.Enemy.Slime
{
    public class SlimeSpawner : ISpawner<Slime>
    {
        public Slime Spawn()
        {
            return new Slime();
        }
    }
}
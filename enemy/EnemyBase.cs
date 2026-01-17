using TombOfOcura.Stat.StatManager;

namespace TombOfOcura.Enemy
{
    public class Enemy : IEnemy
    {
        public string Name { get; protected set; }
        public int Experience { get; protected set; }

        public StatManager StatManager { get; } = new StatManager();

        public Enemy(string name, int exp)
        {
            Name = name;
            Experience = exp;
        }

    }
}
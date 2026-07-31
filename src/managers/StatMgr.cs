
namespace Stats
{
    public enum StatType
    {
        Might, // How well you melee fight
        Arcana, // How well you magic fight
        Fortitude, // How well you resist stuff
        Vitality, // How healthy you are
        Chance, // How lucky you are
    }

    public class StatManager
    {
        private readonly Dictionary<StatType, int> _stats = new();

        public StatManager(IDictionary<StatType, int>? initialStats = null)
        {
            foreach (StatType stat in Enum.GetValues<StatType>())
            {
                _stats[stat] = initialStats != null && initialStats.TryGetValue(stat, out int value)
                    ? value
                    : 10;
            }
        }

        public int Get(StatType stat) => _stats[stat];

        public void Set(StatType stat, int value) => _stats[stat] = value;

        public void Increase(StatType stat, int amount = 1) => _stats[stat] += amount;

        public IReadOnlyDictionary<StatType, int> All => _stats;
    }
}
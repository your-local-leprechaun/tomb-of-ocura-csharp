
namespace TombOfOcura.Stat.StatManager
{
    public class StatManager : IStatManager
    {
        public Dictionary<StatType, Stat> Stats { get; }

        public StatManager()
        {
            Stats = new Dictionary<StatType, Stat>();
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                Stats[statType] = new Stat();
            }
        }

        public Dictionary<StatType, int> GetAllStats()
        {
            Dictionary<StatType, int> allStats = new Dictionary<StatType, int>();
            foreach (var stat in Stats)
            {
                allStats[stat.Key] = stat.Value.Value;
            }

            return allStats;
        }

        public int GetStat(StatType stat)
        {
            return Stats[stat].Value;
        }

        public int GetBaseStat(StatType stat)
        {
            return Stats[stat].BaseValue;
        }

        public void UpdateStat(StatType stat, int amount = 1)
        {
            Stats[stat].UpdateBy(amount);
        }

    }
}

namespace TombOfOcura.Stat.StatManager
{
    public interface IStatManager
    {
        /*
        This interface is for all objects that use stats. This allows
        them to hold stats, modify stats, and retrieve stats.

        Stats are as followed:
        - Focus : How well you fight (increase chance of attack and attack order)
        - Might : How strong you fight (increase attacking damage)
        - Fortitude : How well you take a hit (decreases damage taken)
        - Essence : How well you use magic (increase magic damage)
        - Vigor : How healthy you are (increase max HP + carry capacity for armor)
        */

        Dictionary<StatType, Stat> Stats { get; }

        /*
        Returns all values as a dictionary.
        */
        Dictionary<StatType, int> GetAllStats();

        /*
        Returns value of the requested stat with modifer.
        */
        int GetStat(StatType stat);

        /*
        Returns base value of reqested stat.
        */
        int GetBaseStat(StatType stat);

        /*
        Updates the value of the requested stat by the given amount. Can be negative. Defaults to 1.
        */
        void UpdateStat(StatType stat, int amount = 1);
    }
}
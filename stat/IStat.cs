
namespace TombOfOcura.Stat
{
    public interface IStat
    {
        /*
        Interface for individual stats. Allows for getting base value, modified value, and updating modifier.
        */

        int Value { get; }
        int BaseValue { get; }

        /*
        Update the modifier by the specified amount. Can be negative to decrease the modifier into the negatives.
        Defaults to 1. 
        */
        int UpdateBy(int amount = 1);

        /*
        Updates the base amount of the stat by the specified amount. Cannot go negative. Defualts to 1.
        */
        int UpdateBaseBy(int amount);
    }
}
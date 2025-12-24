
namespace TombOfOcura.Item.Armor
{
    public interface IArmor : IItem
    {
        /*
        Defense value provided by the armor
        */
        int Defense { get; }

        /*
        Required level to equip the armor. Should use Vigor stat
        */
        int RequiredLevel { get; }
    }
}
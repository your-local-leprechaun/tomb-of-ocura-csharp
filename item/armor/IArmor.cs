
namespace TombOfOcura.Item.Armor
{
    public interface IArmor : IItem
    {
        int Defense { get; }
        int RequiredLevel { get; }
    }
}

using Stats;

public class Player
{
    public string Name { get; init; } = "Jayme";

    public StatManager Stats { get; } = new(new Dictionary<StatType, int>
    {
        { StatType.Might, 12},
        { StatType.Arcana, 12},
        { StatType.Fortitude, 12},
        { StatType.Vitality, 12},
        { StatType.Chance, 12},
    });
}
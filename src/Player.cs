
using Stats;
using Basic;
using Returns;

public class Player : Singleton<Player>
{
    private Player() {}

    public static Player Get => Instance;

    public string Name { get; init; } = "Jayme";

    public StatManager Stats { get; } = new(new Dictionary<StatType, int>
    {
        { StatType.Might, 12},
        { StatType.Arcana, 12},
        { StatType.Fortitude, 12},
        { StatType.Vitality, 12},
        { StatType.Chance, 12},
    });

    public EquipmentMgr Equipment { get; } = new();

    public Return Status()
    {
        string returnStr = $"{Name} (Player)\n";

        returnStr += Stats.Status();

        returnStr += Equipment.Status();

        return new Return(returnStr);
    }
}

using Stats;
using Returns;
using Combatants;

public class Player : CombatantBase, ICombatant
{
    private static Player? instance;
    private static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Player)Activator.CreateInstance(typeof(Player), nonPublic: true)!;
            }
            return instance;
        }
    }

    public static Player Get => Instance;

    private Player() : base(
        "Jaymie",
        10
    ) {}

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
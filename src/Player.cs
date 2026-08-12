
using Stats;
using Returns;
using Combatants;
using Commands;
using Parser;
using Enemies;

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
        10,
        new Dictionary<StatType, int>
        {
            { StatType.Might, 12},
            { StatType.Arcana, 12},
            { StatType.Fortitude, 12},
            { StatType.Vitality, 12},
            { StatType.Chance, 12},
        }
    )
    { }

    public Return Status()
    {
        string returnStr = $"{Name} (Player)\n";

        returnStr += Stats.Status();

        returnStr += Equipment.Status();

        return new Return(returnStr);
    }

    public Return PlayerAction(Command command, List<ICombatant> combatants)
    {
        if (command.Verb == "attack")
        {
            // Check if target is in the combatants list.
            ICombatant? enemy = combatants.FirstOrDefault(c => c.Name.ToLower() == command.Noun);

            if (enemy is null)
            {
                throw new CommandException("--Unknown Target--");
            }

            return new Return($"Player attacks {enemy.Name} with {Equipment.EquippedSlot(Items.Equipment.EquipType.Melee)?.ItemName ?? "fists"}");
        }

        throw new CommandException("--Unknown Command--");
    }
}
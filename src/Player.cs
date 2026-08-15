using Stats;
using Returns;
using Combatants;
using Commands;
using Parser;
using Enemies;
using Items.Equipment;

public class Player : CombatantBase, ICombatant
{
    public int PlayerExperience = 0;
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
        0,
        new Dictionary<StatType, int>
        {
            { StatType.Might, 12},
            { StatType.Arcana, 12},
            { StatType.Fortitude, 12},
            { StatType.Vitality, 12},
            { StatType.Chance, 12},
        },
        new Dictionary<EquipType, IEquipment>
        {
            // Debug gear for testing combat without walking to the item each run.
            // { EquipType.Melee, new MeleeBase("Debug Sword", "A sword for debuggers", 99, 4, 6) { IsEquipped = true } },
            { EquipType.Spell, new PoisonMist() }
            // { EquipType.Chest, new ArmorBase("Debug Chest", "A chestplate for debugging", EquipType.Chest, 10) { IsEquipped = true } }
        }
    )
    { }

    public Return Status()
    {
        List<string> ReturnString = [];
        ReturnString.Add($"{Name} (Player)");
        ReturnString.Add($"XP: {PlayerExperience}");
        ReturnString.Add(Stats.Status());
        ReturnString.Add(Equipment.Status());

        return new Return(string.Join("\n", ReturnString));
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

            // Enemy is in target range! Try to hit, and then damage if we do hit! This is for melee only
            // First, let's grab our melee weapon object, or use the fist objects if needed.
            IEquipment? equipped = Equipment.EquippedSlot(EquipType.Melee);
            IMelee weapon = equipped as IMelee ?? new Fist();

            // Now that we have our melee weapon, let's swing to see if we hit the creature.
            if (!(weapon.TryHit() || _hold))
            {
                return new Return($"Player misses {enemy.Name} using {weapon.ItemName}");
            }
            _hold = false;

            // Roll damage, and pass to enemy. Then recieve how much damage we actually did and what health the enemy is now at.
            int damage = weapon.CalcDamage();
            int dealtDamage = enemy.Damage(damage);

            if (enemy.CurrHealth <= 0)
            {
                PlayerExperience += enemy.Experience;
                return new Return($"Player kills {enemy.Name}. {enemy.Experience}xp gained.");
            }

            return new Return($"Player hits {enemy.Name} for {dealtDamage}\n{enemy.Name} HP({enemy.CurrHealth}/{enemy.MaxHealth})");
        }
        else if (command == new Command("hold", "action"))
        {
            Hold();
            return new Return($"Player preps for their next attack.");
        }
        else if (command.Verb == "check")
        {
            IEnemy? enemy = combatants.FirstOrDefault(c => c.Name.ToLower() == command.Noun) as IEnemy ?? null;

            if (enemy is null)
            {
                throw new CommandException("--Unknown Target--");
            }

            return new Return(enemy.Status(), earlyReturn: true);
        }
        else if (command.Verb == "cast")
        {
            IEnemy? enemy = combatants.FirstOrDefault(c => c.Name.ToLower() == command.Noun) as IEnemy ?? null;

            if (enemy is null)
            {
                throw new CommandException("--Unknown Target--");
            }

            // Grab our current spell
            ISpell? spell = Equipment.EquippedSlot(EquipType.Spell) as ISpell ?? null;

            if (spell is null)
            {
                throw new CommandException("--No Spell Equipped--");
            }

            // Put spell on enemy now
            enemy.Conditions.Apply(spell.Status);

            return new Return($"{enemy.Name} has been {spell.Status.Name}!");
        }

        throw new CommandException("--Unknown Command--");
    }
}
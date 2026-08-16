using Combatants;
using Commands;
using Enemies;
using Items;
using Parser;
using Returns;
using State;
using Stats;

class CombatManager : IState
{
    public string Name => "Combat";

    /// <summary>
    /// So this one is a bit more complex. We take in a command, attacking (Melee or magic), holding,
    /// and opening inventory to equip and stuff.
    /// After we deal with that command, we add the response message to a long string that will eventually
    /// be sent back up. 
    /// We then take each monster in order and call their TakeAction and add the response to the return message
    /// so we have a long string by the end.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="CommandException"></exception>
    public Return Execute(Command command)
    {
        List<string> ReturnMessage = [];

        // Take in Player Action
        Return result = Player.Get.PlayerAction(command, _combatants);

        // Add to Return Message
        ReturnMessage.Add(result.Message);

        if (result.EarlyReturn == true)
        {
            return new Return(string.Join("\n", ReturnMessage), combatants: _combatants);
        }

        ReturnMessage.AddRange(CullEnemies());

        // Check if only player is left (win condition)
        if (_combatants.Count == 1)
        {
            // Defeated enemies are gone for good, so clear them from the room
            // that spawned this fight or it'll trigger combat again on re-entry.
            _roomEnemies.Clear();
            ReturnMessage.Add("Player Wins!");
            return new Return(string.Join("\n", ReturnMessage), previous: true);
        }

        NextTurn();

        // Loop through list till Player is active again.
        while (true)
        {
            ICombatant enemy = _combatants[_initiative];
            // If we've gotten back to Player, break free and return all attacks
            if(enemy is Player)
            {
                break;
            }

            // Otherwise, let's continue the loop
            ReturnMessage.Add($"{enemy.Name}'s turn");

            result = enemy.TakeAction(_combatants);
            ReturnMessage.Add(result.Message);

            ReturnMessage.AddRange(CullEnemies());

            // Check if the player is dead
            if (Player.Get.CurrHealth <= 0)
            {
                ReturnMessage.Add("\n**YOU DIED**\n");
                return new Return(string.Join("\n", ReturnMessage), new DeathState());
            }

            // Check if only player is left (win condition) - an enemy may have
            // died from a status effect (e.g. poison) ticking on its own turn.
            if (_combatants.Count == 1)
            {
                _roomEnemies.Clear();
                ReturnMessage.Add("Player Wins!");
                return new Return(string.Join("\n", ReturnMessage), previous: true);
            }

            NextTurn();
        }

        // Add player turn shown. Could add it as a
        ReturnMessage.Add("Player's Turn");

        return new Return(string.Join("\n", ReturnMessage), combatants: _combatants);
    }

    /// <summary>
    /// This one needs to loop through them in case the order is without the player first
    /// </summary>
    /// <returns></returns>
    public Return Activate()
    {
        List<string> ReturnMessage = [];
        ReturnMessage.Add($"\nEntering Combat!\nFighters: {string.Join(", ", _combatants.Select(c => c.Name))}");

        ICombatant enemy = _combatants[_initiative];
        while (enemy is not Player)
        {
            ReturnMessage.Add($"{enemy.Name}'s Turn");
            Return response = enemy.TakeAction(_combatants);
            ReturnMessage.Add(response.Message);

            NextTurn();
            enemy = _combatants[_initiative];
        }

        ReturnMessage.Add("Player's Turn");

        return new Return(string.Join("\n", ReturnMessage), combatants: _combatants);
    }

    private List<ICombatant> _combatants = new();
    private readonly List<ICombatant> _roomEnemies;
    private int _initiative = 0;

    public CombatManager(List<ICombatant> enemies)
    {
        _roomEnemies = enemies;
        foreach (ICombatant enemy in enemies)
        {
            _combatants.Add(enemy);
        }
        _combatants.Add(Player.Get);
        ReorderCombatants();
    }

    private void NextTurn()
    {
        _initiative++;
        if (_initiative > _combatants.Count - 1)
        {
            _initiative = 0;
        }
    }

    /// <summary>
    /// Reorganize _combatants based on stat Vitality, highest goes first.
    /// </summary>
    private void ReorderCombatants()
    {
        _combatants = _combatants
            .OrderByDescending(c => c.Stats.Get(StatType.Vitality))
            .ToList();
    }

    /// <summary>
    /// Remove all enemies with health below or equal to 0, rolling their drops
    /// into the player's inventory first. Returns messages announcing any drops.
    /// </summary>
    private List<string> CullEnemies()
    {
        List<string> messages = new();
        ICombatant current = _combatants[_initiative];

        var dead = _combatants.Where(c => c.CurrHealth <= 0 && c is not Player);
        foreach (ICombatant enemy in dead)
        {
            if (enemy is not IEnemy dyingEnemy)
            {
                continue;
            }

            foreach (IItem item in dyingEnemy.RollDrops())
            {
                Inventory.Get.AddItem(item);
                messages.Add($"{enemy.Name} dropped {item.ItemName}!");
            }
        }

        _combatants.RemoveAll(enemy => enemy.CurrHealth <= 0 && enemy is not Player);

        // Removing dead combatants can shift everyone's index, so re-find whoever's
        // turn it currently is instead of trusting the old _initiative value.
        _initiative = Math.Max(0, _combatants.IndexOf(current));

        return messages;
    }
}
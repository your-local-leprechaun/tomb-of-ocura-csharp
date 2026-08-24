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
    private List<ICombatant> _combatants = new();
    private readonly List<ICombatant> _roomEnemies;
    private int _initiative = 0;
    private bool _browsingInventory = false;

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

        // Player Input is taken in, player turn
        if (command != new Command("skip", "input"))
        {
            // If Inventory is open, do inventory stuff
            if (_browsingInventory)
            {
                if (command.Verb == "check" && !_combatants.Any(c => c.Name.ToLower() == command.Noun))
                {
                    return new Return(Inventory.Get.Examine(command).Message, earlyReturn: true, equipment: true);
                }
                else if (command == new Command("close", "inventory"))
                {
                    _browsingInventory = false;
                    return new Return("You tuck your things away and refocus on the fight.", earlyReturn: true, combatants: _combatants);
                }
                else if (command.Verb == "equip")
                {
                    _browsingInventory = false;
                    ReturnMessage.Add(Inventory.Get.Equip(command).Message);
                }
                else if (command.Verb == "unequip")
                {
                    _browsingInventory = false;
                    ReturnMessage.Add(Inventory.Get.Unequip(command).Message);
                }
                else if (command.Verb == "use")
                {
                    _browsingInventory = false;
                    ReturnMessage.Add(Inventory.Get.Use(command).Message);
                }
                else
                {
                    throw new CommandException("-Unknown Inventory Action-");
                }
            }
            else
            {
                // Outside of combat inventory actions
                if (command == new Command("open", "inventory") ||
                command == new Command("show", "inventory") ||
                command == new Command("show", "items"))
                {
                    _browsingInventory = true;
                    return new Return(Inventory.Get.List().Message, earlyReturn: true, equipment: true);
                }
                else if (command.Verb == "check" && _combatants.Any(c => c.Name.ToLower() == command.Noun))
                {
                    IEnemy enemy = (IEnemy)_combatants.First(c => c.Name.ToLower() == command.Noun);
                    return new Return(enemy.Status(), earlyReturn: true, combatants: _combatants);
                }

                Return result = Player.Get.PlayerAction(command, _combatants);

                // Add to Return Message
                ReturnMessage.Add(result.Message);

                if (result.EarlyReturn == true)
                {
                    return result.Equipment == true
                        ? new Return(string.Join("\n", ReturnMessage), equipment: true)
                        : new Return(string.Join("\n", ReturnMessage), combatants: _combatants);
                }

                // Check if only player is left (win condition)
                if (_combatants.Count == 1)
                {
                    _roomEnemies.Clear();
                    ReturnMessage.Add("Player Wins!");
                    return new Return(string.Join("\n", ReturnMessage), previous: true);
                }
            }
        }
        // Enemy turn is "skip input" was shown
        else
        {
            IEnemy? enemy = _combatants[_initiative] as IEnemy;

            if (enemy == null)
            {
                throw new Exception("No Enemy");
            }

            Return result = enemy.TakeAction(_combatants);

            ReturnMessage.Add(result.Message);
        }

        CullEnemies();

        // Check for win condition
        if (_combatants.Count() == 1)
        {
            _roomEnemies.Clear();
            ReturnMessage.Add("Player Wins!");
            return new Return(string.Join("\n", ReturnMessage), previous: true);
        }

        // Check if death condition
        if (Player.Get.CurrHealth <= 0)
        {
            ReturnMessage.Add("\n**YOU DIED**\n");
            return new Return(string.Join("\n", ReturnMessage), new DeathState());
        }

        NextTurn();

        bool SkipInput = false;
        // Add next turn to return message and set if enemy continues
        if (_combatants[_initiative] == Player.Get)
        {
            ReturnMessage.Add("Player's Turn");
        }
        else
        {
            SkipInput = true;
            ReturnMessage.Add($"{_combatants[_initiative].Name}'s turn");
        }

        return new Return(string.Join("\n", ReturnMessage), combatants: _combatants, skipInput: SkipInput);
    }

    /// <summary>
    /// This one needs to loop through them in case the order is without the player first
    /// </summary>
    /// <returns></returns>
    public Return Activate()
    {
        List<string> ReturnMessage = [];
        ReturnMessage.Add($"\nEntering Combat!");

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
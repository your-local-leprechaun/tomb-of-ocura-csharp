using Combatants;
using Commands;
using Parser;
using Returns;
using State;
using Stats;

class CombatManager : IState
{
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
        try
        {
            // Take in Player Action
            Return result = Player.Get.PlayerAction(command, _combatants);

            // Add to Return Message
            ReturnMessage.Add(result.Message);
            
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

                result = enemy.TakeAction();
                ReturnMessage.Add(result.Message);

                NextTurn();
            }

            // Add player turn shown. Could add it as a 
            ReturnMessage.Add("Player's Turn");

            return new Return(string.Join("\n", ReturnMessage));
        }
        catch (CommandException e)
        {
            throw new CommandException(e.Message);
        }
    }

    /// <summary>
    /// This one needs to loop through them in case the order is without the player first
    /// </summary>
    /// <returns></returns>
    public Return Activate()
    {
        List<string> ReturnMessage = [];
        ReturnMessage.Add($"Entering Combat!\nFighters: {string.Join(", ", _combatants.Select(c => c.Name))}");

        ICombatant enemy = _combatants[_initiative];
        while (enemy is not Player)
        {
            ReturnMessage.Add($"{enemy.Name}'s Turn");
            Return response = enemy.TakeAction();
            ReturnMessage.Add(response.Message);

            NextTurn();
            enemy = _combatants[_initiative];
        }

        ReturnMessage.Add("Player's Turn");

        return new Return(string.Join("\n", ReturnMessage));
    }

    private List<ICombatant> _combatants = new();
    private int _initiative = 0;

    public CombatManager(List<ICombatant> enemies)
    {
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
}
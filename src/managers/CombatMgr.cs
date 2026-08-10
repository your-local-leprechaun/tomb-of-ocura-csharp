using Combatants;
using Commands;
using Returns;
using State;

class CombatManager : IState
{
    public Return Execute(Command command)
    {
        string tempName = _combatants[_initiative].Name;
        NextTurn();
        return new Return($"It's {tempName}'s turn!");
    }

    public Return Activate()
    {
        string response = string.Join(", ", _combatants.Select(c => c.Name));
        return new Return($"Entering Combat!\nFighters: {response}");
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
    /// Reorganize _combatants based on stat Vitalitys
    /// </summary>
    private void ReorderCombatants()
    {
        _combatants.Sort((x, y) => x.Stat)
    }
}
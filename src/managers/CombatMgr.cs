using Combatants;
using Commands;
using Returns;
using State;

class CombatManager : IState
{
    public Return Execute(Command command)
    {
        return new Return("Entering Combat!");
    }

    public Return Activate()
    {
        return new Return("You are now entering combat.");
    }

    private List<CombatantBase> _combatants = new();

    public CombatManager(List<CombatantBase> enemies)
    {
        foreach (CombatantBase enemy in enemies)
        {
            _combatants.Add(enemy);
        }
        _combatants.Add(Player.Get);
    }
}
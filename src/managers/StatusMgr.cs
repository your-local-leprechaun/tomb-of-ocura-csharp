using Combatants;
using Returns;
using Status;

public class StatusManager
{
    List<IStatus> Statuses = [];

    public void Apply(IStatus status)
    {
        IStatus? existing = Statuses.FirstOrDefault(s => s.GetType() == status.GetType());
        if (existing != null)
        {
            existing.ExtendTime(status.Time);
            return;
        }

        Statuses.Add(status);
    }

    // Calls all 
    public Return Aflict(ICombatant target)
    {
        List<string> ReturnMessage = [];

        foreach (IStatus status in Statuses.ToList())
        {
            if (status.Time <= 0)
            {
                Statuses.Remove(status);
                ReturnMessage.Add($"{target.Name} is no longer {status.Name}");
                break;
            }
            Return response = status.Inflict(target);
            ReturnMessage.Add(response.Message);
            if (target.CurrHealth <= 0)
            {
                break;
            }
        }

        return new Return(string.Join("\n", ReturnMessage));
    }
}

public abstract class Room : IRoom
{
    public required string Description { get; init;}
    public required string[] Items;

    public void RemoveItem()
    {
        return;
    }

    public void AddItem()
    {
        return;
    }

    public string[] GetChoices()
    {
        return [];
    }

    public bool RemoveChoice(string toRemove)
    {
        return true;
    }

    public void AddChoice(string toAdd)
    {
        return;
    }

    public void AddEnemies()
    {
        return;
    }

    public void RemoveEnemy()
    {
        return;
    }

    public abstract void RespawnEnemies();
}
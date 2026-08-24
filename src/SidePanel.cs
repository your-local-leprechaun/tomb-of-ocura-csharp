using System.Text.RegularExpressions;
using Combatants;

public static class SidePanel
{
    public static string CustomPanel(string text) => text;

    public static string Basic()
    {
        Player p = Player.Get;
        return $"{p.Name}\nLevel {p.Level}\nXP: {p.PlayerExperience}\nHP: {p.CurrHealth}/{p.MaxHealth}\n\n{p.Stats.Status()}";
    }

    public static string Blank()
    {
        return "";
    }

    public static string EquipmentPanel()
    {
        Player p = Player.Get;
        return $"{p.Name}\nLevel {p.Level}\nXP: {p.PlayerExperience}\nHP: {p.CurrHealth}/{p.MaxHealth}\n\n{p.Equipment.Status()}";
    }

    public static string CombatPanel(List<ICombatant> combatants)
    {
        return string.Join("\n\n", combatants.Select(c =>
            $"{c.Name}\nLvl {c.Level} {Regex.Replace(c.GetType().Name, @"(?<!^)(?=[A-Z])", " ")}\n{c.CurrHealth}/{c.MaxHealth}"));
    }
}

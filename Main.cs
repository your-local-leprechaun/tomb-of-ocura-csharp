/*
This script is meant to be the main file to run that starts the rest of the program. By calling
this, the program as a whole will start
*/
using Utilities;
using TombOfOcura.Rooms;
using TombOfOcura.Item.Consumable;
using TombOfOcura.Item.Armor;
using TombOfOcura.Item.AttackItem.Melee;
using TombOfOcura.Item.AttackItem.Spell;
using TombOfOcura.Stat.StatManager;
using TombOfOcura.Stat;
using TombOfOcura.Enemy;
using TombOfOcura.Enemy.Slime;
using System.Text.Json;

namespace Main
{
    class MainRun()
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            General.PrintOut("Main program starting...");
            Console.ResetColor();

            // Testing Rooms
            IRoom room = new Room1();
            General.PrintOut(room.Name);
            General.PrintOut(room.Description);

            General.PrintOut("Choices:");
            List<string> choices = room.GetChoices();
            foreach (string choice in choices)
            {
                General.PrintOut(choice);
            }

            // Testing Consumable Item
            Consumable consumable = new ConsumableExample();
            General.PrintOut($"Item: {consumable.Name}");
            General.PrintOut($"Description: {consumable.Description}");
            General.PrintOut($"Type: {consumable.Type}");
            consumable.Consume();

            Console.WriteLine();
            // Testing Armor
            Armor armor = new ArmorExample();
            General.PrintOut($"Item: {armor.Name}");
            General.PrintOut($"Description: {armor.Description}");
            General.PrintOut($"Type: {armor.Type}");
            General.PrintOut($"Defense: {armor.Defense}");
            General.PrintOut($"Required Level: {armor.RequiredLevel} Vigor");

            Console.WriteLine();
            // Testing Melee Attack Item
            Melee melee = new MeleeExample();
            General.PrintOut($"Item: {melee.Name}");
            General.PrintOut($"Description: {melee.Description}");
            General.PrintOut($"Type: {melee.Type}");
            melee.Attack();

            Console.WriteLine();
            // Testing Spell Attack Item
            Spell spell = new SpellExample();
            General.PrintOut($"Item: {spell.Name}");
            General.PrintOut($"Description: {spell.Description}");
            General.PrintOut($"Type: {spell.Type}");
            spell.Attack();

            Console.WriteLine();
            // Testing Stat Manager
            StatManager statManager = new StatManager();
            General.PrintOut("Initial Stats:");
            foreach (var stat in statManager.GetAllStats())
            {
                General.PrintOut($"{stat.Key}: {stat.Value}");
            }

            General.PrintOut("Updating Focus by 5...");
            statManager.UpdateStat(StatType.Focus, 5);
            General.PrintOut($"New Focus: {statManager.GetStat(StatType.Focus)}");

            Console.WriteLine();
            // Testing Enemy (Slime)
            ISpawner<Slime> slimeSpawner = new SlimeSpawner();
            Enemy slime = slimeSpawner.Spawn();
            General.PrintOut($"Enemy: {slime.Name}");
            General.PrintOut($"Experience: {slime.Experience}xp");
            General.PrintOut($"Stats: {JsonSerializer.Serialize(slime.StatManager.GetAllStats())}");
        }   
    }
}
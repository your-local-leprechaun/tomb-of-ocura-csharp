/*
This script is meant to be the main file to run that starts the rest of the program. By calling
this, the program as a whole will start
*/
using Utilities;
using TombOfOcura.Rooms;
using TombOfOcura.Item.Consumable;
using TombOfOcura.Item.Armor;
using TombOfOcura.Item.AttackItem.Melee;

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
        }   
    }
}
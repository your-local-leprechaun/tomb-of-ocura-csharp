using Basic;
using Commands;
using Items;
using Parser;
using Returns;
using State;


using System.Text;

namespace Rooms
{
    public class RoomBase<T> : Singleton<T>, IState, IRoom
        where T : RoomBase<T>
    {
        private readonly Dictionary<Command, Func<Return>> _handlers = new();

        // Singleton Information
        protected RoomBase()
        {
            RegisterHandler(new Command("show", "room"), () => new Return(Description), showChoice: false);
            RegisterHandler(new Command("open", "inventory"), () => new Return("Opening Inventory...", Inventory.Get), showChoice: false);
            RegisterHandler(new Command("check", "choices"), () => new Return(string.Join("\n", Choices)), showChoice: false);
            RegisterHandler(new Command("status", "room"), RoomVitals, showChoice: false);
        }

        public static T Get => Instance;

        // State Information
        public Return Execute(Command command)
        {
            if (_handlers.TryGetValue(command, out var handler))
            {
                return handler();
            }

            // Return an Unknown
            throw new CommandException("--Unknown Command--");
        }

        public virtual Return Activate()
        {
            return new Return(RoomName + "\n" + Description);
        }

        // Room Information
        public string RoomName { get; init; } = "";
        public string Description { get; private set; } = "";
        public List<string> Choices { get; private set; } = [];
        public List<IItem> Items { get; private set; } = [];

        protected void RegisterHandler(Command command, Func<Return> handler, bool showChoice = true, string? displayText = null)
        {
            _handlers[command] = handler;
            Parser.Parser.RegisterNoun(command.Noun);
            if (showChoice)
            {
                AddChoice(displayText ?? $"{command.Verb} {command.Noun}");
            }
        }

        protected void UnregisterHandler(Command command, string? displayText = null)
        {
            _handlers.Remove(command);
            RemoveChoice(displayText ?? $"{command.Verb} {command.Noun}");
        }

        protected void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        protected void AddChoice(string newChoice)
        {
            Choices.Add(newChoice);
        }

        protected void RemoveChoice(string removeChoice)
        {
            Choices.Remove(removeChoice);
        }

        protected bool ContainsChoice(string choice)
        {
            return Choices.Contains(choice);
        }

        protected void AddItem(IItem item)
        {
            Items.Add(item);
        }

        protected void RemoveItem<TItem>() where TItem : IItem
        {
            var item = Items.FirstOrDefault(i => i is TItem);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        protected void RemoveItem(IItem item)
        {
            Items.Remove(item);
        }

        protected void CollectItem<TItem>() where TItem : IItem
        {
            // Check if Item is in the room
            IItem? item = Items.FirstOrDefault(i => i is TItem);
            if (item == null)
            {
                return;
            }

            // Add to Inventory
            Inventory.Get.AddItem(item);

            // Remove from room
            RemoveItem(item);
        }

        private Return RoomVitals()
        {
            StringBuilder vitals = new StringBuilder();

            // Room Name
            vitals.AppendLine($"Name: {RoomName}");

            // Description
            vitals.AppendLine($"Description: {Description}");

            // All Choices
            vitals.AppendLine("Choices:");
            if (Choices.Count == 0)
            {
                vitals.AppendLine("  (none)");
            }
            else
            {
                foreach (string choice in Choices)
                {
                    vitals.AppendLine($"  - {choice}");
                }
            }

            // All Handlers (includes hidden/non-choice commands)
            vitals.AppendLine("Handlers:");
            if (_handlers.Count == 0)
            {
                vitals.AppendLine("  (none)");
            }
            else
            {
                foreach (Command command in _handlers.Keys)
                {
                    string adjectivePart = command.Adjective is null ? "" : $" {command.Adjective}";
                    vitals.AppendLine($"  - {command.Verb}{adjectivePart} {command.Noun}");
                }
            }

            // All Items
            vitals.AppendLine("Items:");
            if (Items.Count == 0)
            {
                vitals.AppendLine("  (none)");
            }
            else
            {
                foreach (IItem item in Items)
                {
                    vitals.AppendLine($"  - {item.ItemName}");
                }
            }

            // All Monsters (placeholder until monsters exist)
            vitals.AppendLine("Monsters:");
            vitals.AppendLine("  (none)");

            return new Return(vitals.ToString());
        }
    }
}
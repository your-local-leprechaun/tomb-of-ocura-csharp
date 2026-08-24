using Commands;
using Items;
using Returns;
using Basic;
using Parser;
using State;
using System.Text;
using System.Runtime.CompilerServices;
using Combatants;
using Stats;
using Frontend;

namespace Rooms
{
    /// <summary>
    /// What gets written to / read from the save file for a single room.
    /// ItemTypes/Flags are plain strings so this round-trips through JSON with
    /// no custom converters needed.
    /// </summary>
    public class RoomSaveData
    {
        public List<string> ItemTypes { get; set; } = [];
        public List<string> Flags { get; set; } = [];
    }

    public interface IRoom
    {
        string Description { get; }
        List<string> Choices { get; }
        List<IItem> Items { get; }
        bool NeedsRespawn { get; }
        void Respawn();
        void ClearEnemies();
        RoomSaveData Save();
        void Load(RoomSaveData data);
    }

    public class RoomBase<T> : Singleton<T>, IState, IRoom
        where T : RoomBase<T>
    {
        private readonly Dictionary<Command, Func<Return>> _handlers = new();

        // Singleton Information
        protected RoomBase(string roomName, string description)
        {
            RoomName = roomName;
            Description = description;

            // Shared Choices
            RegisterHandler(new Command("show", "room"), () => new Return(Description), showChoice: false);
            RegisterHandler(new Command("open", "inventory"), () => new Return("", Inventory.Get), showChoice: false);
            RegisterHandler(new Command("check", "choices"), () => new Return(string.Join("\n", Choices)), showChoice: false);
            RegisterHandler(new Command("status", "room"), RoomVitals, showChoice: false);
        }

        public static T Get => Instance;

        public string Name => RoomName;

        // State Information
        public Return Execute(Command command)
        {
            if (_handlers.TryGetValue(command, out var handler))
            {
                return handler();
            }

            // Return an Unknown
            throw new UnknownCommandException(command);
        }

        public virtual Return Activate()
        {
            // If Enemies are in the room, start a battle.
            if (Enemies.Count > 0)
            {
                List<ICombatant> combatants = [];
                combatants.AddRange(Enemies);
                combatants.Add(Player.Get);

                combatants = combatants
                    .OrderByDescending(c => c.Stats.Get(StatType.Vitality))
                    .ToList();

                // Start a Combat instance with Enemies
                return new Return($"\nEntering {RoomName}, you find enemies!", new CombatManager(Enemies), sidePanel: SidePanel.CombatPanel(combatants));
            }

            // Else Return the normal description stuff.
            return new Return("\n" + Description);
        }

        // Room Information
        public string RoomName { get; init; } = "";
        public string Description { get; private set; } = "";
        public List<string> Choices { get; private set; } = [];
        public List<IItem> Items { get; private set; } = [];
        public List<ICombatant> Enemies { get; private set; } = [];

        public bool NeedsRespawn { get; private set; } = false;

        public virtual void Respawn()
        {
            // Respawn enemies here!
            return;
        }

        public void ClearEnemies()
        {
            Enemies.Clear();
        }

        // Save/Load
        //
        // Description/Choices/_handlers are never serialized directly - they're
        // rebuilt as a side effect of Replay() re-calling whichever transition
        // methods already fired (see MarkDone/IsDone below), the same way they
        // were built the first time the player took those actions.
        private readonly HashSet<string> _flags = new();

        /// <summary>
        /// Call at the end of a state-transition method (e.g. GrabKey, OpenDoor)
        /// to record that it has completed, so Replay() can re-fire it on load.
        /// Takes no argument on purpose - CallerMemberName means there's no
        /// string to typo or let drift from the method it's marking.
        /// </summary>
        protected void MarkDone([CallerMemberName] string? method = null)
        {
            _flags.Add(method!);
        }

        protected bool IsDone(string method) => _flags.Contains(method);

        public virtual RoomSaveData Save()
        {
            return new RoomSaveData
            {
                ItemTypes = Items.Select(i => i.GetType().Name).ToList(),
                Flags = _flags.ToList()
            };
        }

        public virtual void Load(RoomSaveData data)
        {
            Items.Clear();
            foreach (string typeName in data.ItemTypes)
            {
                Items.Add(ItemFactory.Create(typeName));
            }

            _flags.Clear();
            foreach (string flag in data.Flags)
            {
                _flags.Add(flag);
            }

            Replay();
        }

        /// <summary>
        /// Re-fires whichever transition methods MarkDone recorded, in the order
        /// they need to happen in, so Description/Choices/_handlers end up back
        /// where they were. Rooms with no branching state (most of them) never
        /// need to override this - the default no-op is already correct.
        /// </summary>
        protected virtual void Replay() { }

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

        protected void AddEnemy(Func<ICombatant> factory)
        {
            Enemies.Add(factory());
            if (!NeedsRespawn)
            {
                RoomRegistry.Register(this);
                NeedsRespawn = true;
            }
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

    public class RoomRegistry
    {
        private static readonly List<IRoom> _rooms = [];
        public static void Register(IRoom room) => _rooms.Add(room);
        public static IRoom Checkpoint = Room1.Get;

        public static void RespawnTouched()
        {
            List<IRoom> touched = _rooms.Where(r => r.NeedsRespawn).ToList();

            foreach (IRoom room in touched)
            {
                room.ClearEnemies();
                room.Respawn();
            }
        }

        public static void UpdateCheckpoint(IRoom? room)
        {
            if (room is null)
            {
                return;
            }
            Checkpoint = room;
        }
    }
}
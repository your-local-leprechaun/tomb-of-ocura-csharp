using System.Reflection;

namespace Items
{
    /// <summary>
    /// Reconstructs an IItem from its type name for loading a save file. Looks
    /// types up by reflection instead of a hand-maintained switch/dictionary,
    /// so a new item class works automatically - nothing to register here when
    /// you add one.
    /// </summary>
    public static class ItemFactory
    {
        private static readonly Dictionary<string, Type> _types =
            Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IItem).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .ToDictionary(t => t.Name);

        public static IItem Create(string typeName)
        {
            if (!_types.TryGetValue(typeName, out Type? type))
            {
                throw new InvalidOperationException($"No item type named '{typeName}' - was it renamed since this save was made?");
            }

            return (IItem)Activator.CreateInstance(type)!;
        }
    }
}

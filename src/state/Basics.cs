
namespace Basic
{
    public abstract class Singleton<T> where T : class
    {
        private static T? instance;
        protected static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)Activator.CreateInstance(typeof(T), nonPublic: true)!;
                }
                return instance;
            }
        }
    }
}

public readonly record struct Range(int Min, int Max)
{
    public int Roll(Random rng) => rng.Next(Min, Max + 1);
}
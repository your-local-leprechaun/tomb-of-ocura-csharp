
namespace TombOfOcura.Stat
{
    public class Stat
    {
        private int _baseValue;
        private int _modifier;

        public int Value => _baseValue + _modifier;
        public int BaseValue => _baseValue;

        public Stat()
        {
            _baseValue = 1;
            _modifier = 0;
        }

        public void UpdateBy(int amount = 1)
        {
            _modifier += amount;
        }

        public void UpdateBaseBy(int amount = 1)
        {
            _baseValue = Math.Max(0, _baseValue + amount);
        }
    }
}
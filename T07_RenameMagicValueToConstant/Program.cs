namespace T07_RenameMagicValueToConstant
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class Weapon
    {
        private int _bullets;

        public bool CanShoot() => _bullets > 0;

        public void Shoot() => _bullets -= 1;
    }
}

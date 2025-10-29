namespace T07_RenameMagicValueToConstant
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ДЗ: \"10. Магические числа нужно всегда заменять на константы\"";
        }
    }

    public class Weapon
    {
        private const int BulletsPerShot = 1;

        private int _bullets;

        public Weapon(int bullets)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(bullets);

            _bullets = bullets;
        }

        public bool CanShoot => 
            _bullets > BulletsPerShot;

        public void Shoot() => 
            _bullets = CanShoot ? _bullets -= BulletsPerShot: _bullets = default;
    }
}

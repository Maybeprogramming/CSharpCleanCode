namespace T12_GroupFieldsByPrefix
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "20. Группировка полей по префиксу";
        }
    }

    public class Player
    {
        private readonly PlayerInfo _playerInfo;
        private readonly Weapon _weapon;
        private readonly Movement _movement;

        public Player(PlayerInfo playerInfo, Weapon weapon, Movement movement)
        {
            ArgumentNullException.ThrowIfNull(playerInfo, nameof(playerInfo));
            ArgumentNullException.ThrowIfNull(weapon, nameof(playerInfo));
            ArgumentNullException.ThrowIfNull(movement, nameof(playerInfo));

            _playerInfo = playerInfo;
            _weapon = weapon;
            _movement = movement;
        }
    }

    public class PlayerInfo
    {
        public PlayerInfo(string name, int age)
        {
            ArgumentOutOfRangeException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegative(age, nameof(age));

            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }

    public class Movement
    {
        public Movement(float speed)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(speed, nameof(speed));

            Speed = speed;
        }

        public float Speed { get; private set; }

        public void Move(Vector2D targetPosition)
        {
            //Do move
        }
    }

    public class Weapon
    {
        public Weapon(int damage, float cooldown)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(damage, nameof(damage));
            ArgumentOutOfRangeException.ThrowIfNegative(cooldown, nameof(cooldown));

            Damage = damage;
            Cooldown = cooldown;
        }

        public int Damage { get; private set; }
        public float Cooldown { get; private set; }

        public bool IsReloading()
        {
            throw new NotImplementedException();
        }
    }

    public struct Vector2D(float x, float y)
    {
        public float X { get; private set; } = x;
        public float Y { get; private set; } = y;

        public void Set(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
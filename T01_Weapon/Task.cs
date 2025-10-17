namespace T01_Weapon
{
    class Weapon
    {
        private int _damage;
        private int _bullets;

        public Weapon(int damage, int bullets)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(damage);
            ArgumentOutOfRangeException.ThrowIfNegative(bullets);

            _damage = damage;
            _bullets = bullets;
        }

        public void TryFire(Player player)
        {
            if (_bullets <= 0)
                throw new ArgumentOutOfRangeException(nameof(_bullets));

            player.TryTakeDamage(_damage);
            _bullets--;
        }
    }

    class Player
    {
        private int _health;

        public int Health
        {
            get => _health;
            private set => SetHealth(value);
        }

        public bool IsAlive => _health > 0;

        public void TryTakeDamage(int damage)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(damage);

            if (IsAlive)
                Health -= damage;
        }

        private void SetHealth(int value)
        {
            if (value >= 0)
                _health = value;
            else
                _health = 0;
        }
    }

    class Bot
    {
        private Weapon _weapon;

        public Bot(Weapon weapon)
        {
            if (weapon == null)
                throw new NullReferenceException(nameof(weapon));

            _weapon = weapon;
        }

        public void OnSeePlayer(Player player)
        {
            if (player == null)
                throw new NullReferenceException(nameof(player));

            _weapon.TryFire(player);
        }
    }
}
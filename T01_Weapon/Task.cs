namespace T01_Weapon
{
    class Weapon
    {
        private int _damage;
        private int _bullets;

        public void Fire(Player player)
        {
            if (_bullets > 0)
            {
                player.TryTakeDamage(_damage);
                _bullets -= 1;
            }
        }
    }

    class Player
    {
        private int _health;

        public int Health => _health;

        public bool TryTakeDamage(int damage)
        {
            if (damage < 0)
            {
                return false;
            }

            if (_health >= damage)
            {
                _health -= damage;
                return true;
            }
            else
            {
                _health = 0;
                return false;
            }
        }
    }

    class Bot
    {
        private Weapon _weapon;

        public void Initialize(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void OnSeePlayer(Player player)
        {
            _weapon.Fire(player);
        }
    }
}
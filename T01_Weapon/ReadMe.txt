���� ����� �� ����� � ����� ����� ������ �������� � ����.  
������� ����������� ������ ������� �� ������ � ����������� ��������� ������� OnSeePlayer ������� ������� ���-�� ����� ��������.  
  
����������� � ������������� � ���� ���� -�
[https://gist.github.com/HolyMonkey/9290ed63c38fc732ed8f58693077095d]

    class Weapon
    {
        public int Damage;
        public int Bullets;
        
        public void Fire(Player player)
        {
            player.Health -= Damage;
            Bullets -= 1;
        }
    }
    
    class Player
    {
        public int Health;
    }
    
    class Bot
    {
        public Weapon Weapon;
        
        public void OnSeePlayer(Player player)
        {
            Weapon.Fire(player);
        }
    }

������� 1:
###################### �������, 11 ������ 2024, 19:38

1. _bullets -= 1; - 1 - ���������� �����

2. 
class Weapon
{
    private int _damage;
    private int _bullets; 
    - ��� ����� ����������� � ���������� � ��������� ��������

3.
if (damage < 0)
{
    return false;
} 
- � �������������� ��������� �� ����� ������������ return, ������ ����������

4. public void Initialize(Weapon weapon) 
- ��� ������ ���� ����������� � ��������� �� null

������� 2
######################## �������, 17 ������� 2025, 14:53
	
���������� ����� ������� � ����� if, � �� else. 
public void TryFire(Player player) - ���� ���� ���, ������� ������� ����������

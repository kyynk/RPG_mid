namespace BattleState
{
    public class Player
    {
        public int hp;
        public int atk;

        public Player()
        {
            ResetStatus();
        }

        public void ResetStatus()
        {
            // need to use readfile to get the value of hp and atk
            hp = 100;
            atk = 10;
        }

        public void Attack(Player enemy, bool isCritical)
        {
            if (isCritical)
            {
                enemy.TakeDamage(atk * 2);
            }
            else
            {
                enemy.TakeDamage(atk);
            }
        }

        public void Heal(int amount)
        {
            hp += amount;
        }

        public void TakeDamage(int amount)
        {
            hp -= amount;
        }
    }
}

namespace BattleState
{
    public class Player
    {
        public int hp { get; private set; }
        public int atk { get; private set; }
        public bool isDefend {  get; set; }

        public Player()
        {
            ResetStatus();
        }

        public void ResetStatus()
        {
            // need to use readfile to get the value of hp and atk
            hp = 100;
            atk = 10;

            isDefend = false;
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
            if (isDefend)
            {
                hp -= amount / 2;
                isDefend = false;
            }
            else
            {
                hp -= amount;
            }
        }
    }
}

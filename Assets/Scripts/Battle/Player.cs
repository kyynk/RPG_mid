namespace BattleState
{
    public class Player : IPlayer
    {
        public int HP { get; set; }
        public int ATK { get; set; }
        public bool IsDefend {  get; set; }

        public Player()
        {
            ResetStatus();
        }

        public void ResetStatus()
        {
            // need to use readfile to get the value of hp and atk
            HP = 100;
            ATK = 10;

            IsDefend = false;
        }

        public void Attack(IPlayer enemy, bool isCritical)
        {
            if (isCritical)
            {
                enemy.TakeDamage(ATK * 2);
            }
            else
            {
                enemy.TakeDamage(ATK);
            }
        }

        public void Heal(int amount)
        {
            HP += amount;
        }

        public void TakeDamage(int amount)
        {
            if (IsDefend)
            {
                HP -= amount / 2;
                IsDefend = false;
            }
            else
            {
                HP -= amount;
            }
        }
    }
}

namespace RPGBattle
{
    public class Character : ICharacter
    {
        public int HP { get; set; }
        public int ATK { get; set; }
        public bool IsDefend { get; set; }

        public Character()
        {
            ResetStatus();
        }

        public void ResetStatus()
        {
            // HP and ATK values should be read from a file
            HP = 100;
            ATK = 10;
            IsDefend = false;
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
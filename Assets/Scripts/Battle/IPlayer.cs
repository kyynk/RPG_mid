namespace RPGBattle
{
    public interface IPlayer
    {
        Character Character { get; set; }

        void ResetStatus();
        void Attack(IPlayer enemy, bool isCritical);
        void Defend();
        void Heal(int amount);
        void TakeDamage(int amount);
        // Is character HP <= 0
        bool IsCharacterDead();
    }
}
namespace RPGBattle
{
    public interface ICharacter
    {
        int HP { get; set; }
        int ATK { get; set; }
        bool IsDefend { get; set; }
        string Name { get; }

        void ResetStatus();
        void Heal(int amount);
        void TakeDamage(int amount);
    }
}
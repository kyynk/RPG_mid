namespace BattleState
{
    public interface IPlayer
    {
        int HP { get; set; }
        int ATK { get; set; }

        bool IsDefend { get; set; }

        void ResetStatus();
        void Attack(IPlayer enemy, bool isCritical);
        void Heal(int amount);
        void TakeDamage(int amount);
    }
}
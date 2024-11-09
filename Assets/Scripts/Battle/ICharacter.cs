using System.Collections;

namespace RPGBattle
{
    public interface ICharacter
    {
        int HP { get; set; }
        int ATK { get; set; }
        bool IsDefend { get; set; }
        string Name { get; }

        void ResetStatus();
        IEnumerator Attack(bool isCritical);
        IEnumerator Heal(int amount);
        IEnumerator TakeDamage(int amount, bool isEventDamage);
    }
}
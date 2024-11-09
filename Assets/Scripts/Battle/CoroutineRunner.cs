using UnityEngine;

namespace RPGBattle
{
    public class CoroutineRunner : MonoBehaviour
    {
        public void StartAttackCoroutine(Character character, bool isCritical)
        {
            StartCoroutine(character.Attack(isCritical));
        }

        public void StartHealCoroutine(Character character, int amount)
        {
            StartCoroutine(character.Heal(amount));
        }

        public void StartTakeDamageCoroutine(Character character, int amount, bool isEventDamage)
        {
            StartCoroutine(character.TakeDamage(amount, isEventDamage));
        }
    }
}

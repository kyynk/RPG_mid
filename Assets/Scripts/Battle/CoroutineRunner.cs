using System.Collections;
using UnityEngine;

namespace RPGBattle
{
    public class CoroutineRunner : MonoBehaviour
    {
        public IEnumerator StartAttackCoroutine(Character character, bool isCritical)
        {
            yield return StartCoroutine(character.Attack(isCritical));
        }

        public IEnumerator StartHealCoroutine(Character character, int amount)
        {
            yield return StartCoroutine(character.Heal(amount));
        }

        public IEnumerator StartTakeDamageCoroutine(Character character, int amount, bool isEventDamage)
        {
            yield return StartCoroutine(character.TakeDamage(amount, isEventDamage));
        }
    }
}

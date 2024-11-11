using System.Collections;
using UnityEngine;

namespace RPGBattle
{
    public class Player
    {
        public Character PlayerCharacter { get; set; }
        private CoroutineRunner coroutineRunner;
        private HealthBar healthBar;
        private GameObject shield;

        public Player(Character _character, string _healthBarTag, string _shieldTag, CoroutineRunner _coroutineRunner)
        {
            PlayerCharacter = _character;
            GameObject healthBarImg = GameObject.FindGameObjectWithTag(_healthBarTag);
            if (healthBarImg == null)
            {
                Debug.LogError($"Health bar image for {_healthBarTag} not found!");
            }
            GameObject shieldObject = GameObject.FindGameObjectWithTag(_shieldTag);
            if (shieldObject == null)
            {
                Debug.LogError($"Defend image for {_shieldTag} not found!");
            }
            shield = shieldObject;
            shield.SetActive(PlayerCharacter.IsDefend);
            healthBar = new HealthBar(healthBarImg.GetComponent<UnityEngine.UI.Image>());
            healthBar.SetMaxHealth(PlayerCharacter.GetMaxHp());
            coroutineRunner = _coroutineRunner;
        }

        public void ResetStatus()
        {
            PlayerCharacter.ResetStatus();
            healthBar.SetHealth(PlayerCharacter.HP);
            shield.SetActive(PlayerCharacter.IsDefend);
        }

        public IEnumerator Attack(Player enemy, bool isCritical)
        {
            yield return coroutineRunner.StartAttackCoroutine(PlayerCharacter, isCritical);
            int damage = isCritical ? PlayerCharacter.ATK * 2 : PlayerCharacter.ATK;
            yield return enemy.TakeDamage(damage, false);
        }

        public void Defend()
        {
            PlayerCharacter.IsDefend = true;
            shield.SetActive(PlayerCharacter.IsDefend);
        }

        public IEnumerator Heal(int amount)
        {
            yield return coroutineRunner.StartHealCoroutine(PlayerCharacter, amount);
            if (PlayerCharacter.HP > PlayerCharacter.GetMaxHp())
            {
                PlayerCharacter.HP = PlayerCharacter.GetMaxHp();
            }
            healthBar.SetHealth(PlayerCharacter.HP);
        }

        public IEnumerator TakeDamage(int amount, bool isEventDamage)
        {
            yield return coroutineRunner.StartTakeDamageCoroutine(PlayerCharacter, amount, isEventDamage);
            shield.SetActive(PlayerCharacter.IsDefend);
            if (PlayerCharacter.HP < 0)
            {
                PlayerCharacter.HP = 0;
            }
            healthBar.SetHealth(PlayerCharacter.HP);
        }

        public bool IsCharacterDead()
        {
            return PlayerCharacter.HP <= 0;
        }
    }
}

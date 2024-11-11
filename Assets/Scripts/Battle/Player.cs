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
            healthBar.SetMaxHealth(PlayerCharacter.HP);
            coroutineRunner = _coroutineRunner;
        }

        public void ResetStatus()
        {
            PlayerCharacter.ResetStatus();
            healthBar.SetHealth(PlayerCharacter.HP);
            shield.SetActive(PlayerCharacter.IsDefend);
        }

        public void Attack(Player enemy, bool isCritical)
        {
            coroutineRunner.StartAttackCoroutine(PlayerCharacter, isCritical);
            int damage = isCritical ? PlayerCharacter.ATK * 2 : PlayerCharacter.ATK;
            enemy.TakeDamage(damage, false);
        }

        public void Defend()
        {
            PlayerCharacter.IsDefend = true;
            shield.SetActive(PlayerCharacter.IsDefend);
        }

        public void Heal(int amount)
        {
            coroutineRunner.StartHealCoroutine(PlayerCharacter, amount);
            healthBar.SetHealth(PlayerCharacter.HP);
        }

        public void TakeDamage(int amount, bool isEventDamage)
        {
            coroutineRunner.StartTakeDamageCoroutine(PlayerCharacter, amount, isEventDamage);
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

using UnityEngine;

namespace RPGBattle
{
    public class Player : IPlayer
    {
        public Character Character {  get; set; }
        private CoroutineRunner coroutineRunner;
        private HealthBar healthBar;
        private GameObject shieldImg;

        public Player(Character _character, string playerHealthBar, string playerShield, CoroutineRunner _coroutineRunner)
        {
            Character = _character;
            GameObject healthBarImg = GameObject.FindGameObjectWithTag(playerHealthBar);
            if (healthBarImg == null)
            {
                Debug.LogError($"Health bar image for {playerHealthBar} not found!");
            }
            GameObject defendImg = GameObject.FindGameObjectWithTag(playerShield);
            if (defendImg == null)
            {
                Debug.LogError($"Defend image for {playerShield} not found!");
            }
            shieldImg = defendImg;
            shieldImg.SetActive(Character.IsDefend);
            healthBar = new HealthBar(healthBarImg.GetComponent<UnityEngine.UI.Image>());
            healthBar.SetMaxHealth(Character.HP);
            coroutineRunner = _coroutineRunner;
        }

        public void ResetStatus()
        {
            Character.ResetStatus();
            healthBar.SetHealth(Character.HP);
            shieldImg.SetActive(Character.IsDefend);
        }

        public void Attack(IPlayer enemy, bool isCritical)
        {
            coroutineRunner.StartAttackCoroutine(Character, isCritical);
            int damage = isCritical ? Character.ATK * 2 : Character.ATK;
            enemy.TakeDamage(damage, false);
        }

        public void Defend()
        {
            Character.IsDefend = true;
            shieldImg.SetActive(Character.IsDefend);
        }

        public void Heal(int amount)
        {
            coroutineRunner.StartHealCoroutine(Character, amount);
            healthBar.SetHealth(Character.HP);
        }

        public void TakeDamage(int amount, bool isEventDamage)
        {
            coroutineRunner.StartTakeDamageCoroutine(Character, amount, isEventDamage);
            shieldImg.SetActive(Character.IsDefend);
            if (Character.HP < 0)
            {
                Character.HP = 0;
            }
            healthBar.SetHealth(Character.HP);
        }

        public bool IsCharacterDead()
        {
            return Character.HP <= 0;
        }
    }
}

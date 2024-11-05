using UnityEngine;

namespace RPGBattle
{
    public class Player : IPlayer
    {
        public Character Character {  get; set; }
        private HealthBar healthBar;

        public Player(Character _character, string playerHealthBar)
        {
            Character = _character;
            GameObject healthBarImg = GameObject.FindGameObjectWithTag(playerHealthBar);
            if (healthBarImg == null)
            {
                Debug.LogError($"Health bar image for {playerHealthBar} not found!");
            }
            healthBar = new HealthBar(healthBarImg.GetComponent<UnityEngine.UI.Image>());
            healthBar.SetMaxHealth(Character.HP);
        }

        public void ResetStatus()
        {
            Character.ResetStatus();
            healthBar.SetHealth(Character.HP);
        }

        public void Attack(IPlayer enemy, bool isCritical)
        {
            int damage = isCritical ? Character.ATK * 2 : Character.ATK;
            enemy.TakeDamage(damage);
        }

        public void Defend()
        {
            Character.IsDefend = true;
        }

        public void Heal(int amount)
        {
            Character.Heal(amount);
            healthBar.SetHealth(Character.HP);
        }

        public void TakeDamage(int amount)
        {
            Character.TakeDamage(amount);
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

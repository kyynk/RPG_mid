using UnityEngine.UI;

namespace RPGBattle
{
    public class HealthBar
    {
        public Image healthFillImage; // Reference to the health bar Image

        private int maxHealth;

        public HealthBar(Image _healthFillImage)
        {
            healthFillImage = _healthFillImage;
        }

        public void SetMaxHealth(int maxHealth)
        {
            this.maxHealth = maxHealth;
            healthFillImage.fillAmount = 1f; // Set to full at the start
        }

        public void SetHealth(int currentHealth)
        {
            healthFillImage.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}

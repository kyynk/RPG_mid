using System;
using System.IO;
using UnityEngine;

namespace RPGBattle
{
    public class EventHandler
    {
        private System.Random random; // using System.Random, since Unity's Random not random enough
        private bool isCritical;
        private bool isHeal;
        private bool isDamage;

        public EventHandler()
        {
            random = new System.Random();
            isCritical = false;
            isHeal = false;
            isDamage = false;
        }

        public void OnPlayerAttack(Player player, Player enemy)
        {
            float critRate = LoadEventConfigFromFile("crit_rate", player);
            isCritical = random.NextDouble() < critRate;
            player.Attack(enemy, isCritical);
        }

        public void OnPlayerDefend(Player player)
        {
            player.Defend();
        }

        /// <summary>
        /// random event for player to heal
        /// </summary>
        /// <param name="player"></param>
        public void OnPlayerHeal(Player player)
        {
            float healRate = LoadEventConfigFromFile("heal_rate", player);
            isHeal = random.NextDouble() < healRate;
            if (isHeal)
            {
                float healAmount = LoadEventConfigFromFile("heal_amount", player);
                player.Heal((int)healAmount);
            }
        }

        /// <summary>
        /// random event for player to take damage
        /// </summary>
        /// <param name="player"></param>
        public void OnPlayerTakeEventDamage(Player player)
        {
            float damageRate = LoadEventConfigFromFile("damage_rate", player);
            isDamage = random.NextDouble() < damageRate;
            if (isDamage)
            {
                float damageAmount = LoadEventConfigFromFile("damage_amount", player);
                player.TakeDamage((int)damageAmount, true);
            }
        }

        private float LoadEventConfigFromFile(string fileName, Player player)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "EventConfig", fileName + ".csv");

            if (!File.Exists(filePath))
            {
                Debug.LogError($"File {fileName} not found!");
                return 0;
            }
            try
            {
                string[] lines = File.ReadAllLines(filePath);  // Reads all lines from the file
                foreach (string line in lines)
                {
                    string[] values = line.Split(',');
                    // Check if the name matches the first column
                    if (values.Length > 1 && values[0] == player.PlayerCharacter.Name)
                    {
                        if (float.TryParse(values[1], out float targetValue))
                        {
                            return targetValue;  // Return parsed value for HP or ATK
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error reading {fileName}.csv: {ex.Message}");
            }
            Debug.LogWarning($"Value for {player.PlayerCharacter.Name} not found in {fileName}.csv");
            return 0;
        }
    }
}

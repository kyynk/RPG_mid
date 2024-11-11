using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGBattle
{
    public class EventHandler
    {
        private System.Random random; // using System.Random, since Unity's Random not random enough
        private bool isCritical;
        private bool isHeal;
        private bool isDamage;
        private List<string> characterNames;
        private List<List<float>> eventData;

        public EventHandler(List<string> _characterNames)
        {
            random = new System.Random();
            isCritical = false;
            isHeal = false;
            isDamage = false;
            eventData = new List<List<float>>();
            characterNames = new List<string>(_characterNames);
            foreach (var name in characterNames)
            {
                eventData.Add(new List<float>
                {
                    LoadEventConfigFromFile("crit_rate", name),
                    LoadEventConfigFromFile("heal_rate", name),
                    LoadEventConfigFromFile("heal_amount", name),
                    LoadEventConfigFromFile("damage_rate", name),
                    LoadEventConfigFromFile("damage_amount", name)
                });
            }
        }

        public IEnumerator OnPlayerAttack(Player player, Player enemy)
        {
            int whichPlayer = characterNames.FindIndex(x => x == player.PlayerCharacter.Name);
            isCritical = random.NextDouble() < eventData[whichPlayer][0];
            yield return player.Attack(enemy, isCritical);
        }

        public IEnumerator OnPlayerDefend(Player player)
        {
            player.Defend();
            yield return null;
        }

        /// <summary>
        /// random event for player to heal
        /// </summary>
        /// <param name="player"></param>
        public IEnumerator OnPlayerHeal(Player player)
        {
            int whichPlayer = characterNames.FindIndex(x => x == player.PlayerCharacter.Name);
            isHeal = random.NextDouble() < eventData[whichPlayer][1];
            if (isHeal)
            {
                yield return player.Heal((int)eventData[whichPlayer][2]);
            }
        }

        /// <summary>
        /// random event for player to take damage
        /// </summary>
        /// <param name="player"></param>
        public IEnumerator OnPlayerTakeEventDamage(Player player)
        {
            int whichPlayer = characterNames.FindIndex(x => x == player.PlayerCharacter.Name);
            isDamage = random.NextDouble() < eventData[whichPlayer][3];
            if (isDamage)
            {
                yield return player.TakeDamage((int)eventData[whichPlayer][4], true);
            }
        }

        private float LoadEventConfigFromFile(string fileName, string characterName)
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
                    if (values.Length > 1 && values[0] == characterName)
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
            Debug.LogWarning($"Value for {characterName} not found in {fileName}.csv");
            return 0;
        }
    }
}

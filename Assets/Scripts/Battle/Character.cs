using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace RPGBattle
{
    public class Character : ICharacter
    {
        public int HP { get; set; }
        public int ATK { get; set; }
        public bool IsDefend { get; set; }

        public string Name { get; }

        private float critRate;
        private float healRate;
        private int healAmount;

        public Character(string _name)
        {
            Name = _name;
            ResetStatus();
        }

        public void ResetStatus()
        {
            HP = LoadValueFromFile("hp");
            ATK = LoadValueFromFile("atk");
            IsDefend = false;
        }

        private int LoadValueFromFile(string fileName)
        {
            string filePath = Path.Combine(Application.dataPath, "ConfigForGame", "PlayerConfig", fileName + ".csv");
            
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File {fileName} not found!");
                return ReturnDefaultValue(fileName);
            }

            try
            {
                string[] lines = File.ReadAllLines(filePath);  // Reads all lines from the file
                foreach (string line in lines)
                {
                    string[] values = line.Split(',');
                    // Check if the name matches the first column
                    if (values.Length > 1 && values[0] == Name)
                    {
                        if (int.TryParse(values[1], out int targetValue))
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

            Debug.LogWarning($"Value for {Name} not found in {fileName}.csv. Using default value.");
            return ReturnDefaultValue(fileName);
        }

        public int ReturnDefaultValue(string type)
        {
            if (type == "atk")
            {
                return 10;
            }
            else
            {
                return 100;
            }
        }

        public void Heal(int amount)
        {
            HP += amount;
        }

        public void TakeDamage(int amount)
        {
            if (IsDefend)
            {
                HP -= amount / 2;
                IsDefend = false;
            }
            else
            {
                HP -= amount;
            }
        }
    }
}
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace RPGBattle
{
    public class Character : ICharacter
    {
        public int HP { get; set; }
        public int ATK { get; set; }
        public bool IsDefend { get; set; }

        public string Name { get; }

        private Animator characterAnimator;
        private Animator healAnimator;
        private Animator boomAnimator;

        public Character(string _name)
        {
            Name = _name;
            GameObject gameObject = GameObject.FindGameObjectWithTag(Name);
            if (gameObject == null)
            {
                Debug.LogError($"GameObject for {Name} not found!");
            }
            characterAnimator = gameObject.GetComponent<Animator>();
            gameObject = GameObject.FindGameObjectWithTag(Name + "Heal");
            if (gameObject == null)
            {
                Debug.LogError($"GameObject for {Name}Heal not found!");
            }
            healAnimator = gameObject.GetComponent<Animator>();
            gameObject = GameObject.FindGameObjectWithTag(Name + "Boom");
            if (gameObject == null)
            {
                Debug.LogError($"GameObject for {Name}Boom not found!");
            }
            boomAnimator = gameObject.GetComponent<Animator>();
            ResetStatus();
        }

        public void ResetStatus()
        {
            HP = LoadValueFromFile("hp");
            ATK = LoadValueFromFile("atk");
            IsDefend = false;
            characterAnimator.Play("idle");
            healAnimator.Play("hidden");
            boomAnimator.Play("hidden");
        }

        public IEnumerator Attack(bool isCritical)
        {
            if (isCritical)
            {
                characterAnimator.Play("crit_attack");
            }
            else
            {
                characterAnimator.Play("attack");
            }
            while (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack") &&
                   characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
            {
                yield return null;  // Wait until the next frame
            }
        }

        public IEnumerator Heal(int amount)
        {
            healAnimator.Play("heal");
            HP += amount;
            yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        public IEnumerator TakeDamage(int amount, bool isEventDamage)
        {
            if (isEventDamage)
            {
                boomAnimator.Play("boom");
            }
            characterAnimator.Play("injure");

            if (IsDefend)
            {
                HP -= amount / 2;
                IsDefend = false;
            }
            else
            {
                HP -= amount;
            }

            while (characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("injure") &&
                   characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
        }

        private int LoadValueFromFile(string fileName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "PlayerConfig", fileName + ".csv");

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

        private int ReturnDefaultValue(string type)
        {
            return type == "atk" ? 10 : 100;
        }
    }
}
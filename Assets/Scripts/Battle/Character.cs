using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace RPGBattle
{
    public class Character
    {
        public int HP { get; set; }
        public int ATK { get; set; }
        public bool IsDefend { get; set; }

        public string Name { get; }

        private int defaultHp;
        private int defaultAtk;
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
            SetDefaultValues();
            ResetStatus();
        }

        private void SetDefaultValues()
        {
            defaultHp = LoadValueFromFile("hp");
            defaultAtk = LoadValueFromFile("atk");
        }

        public void ResetStatus()
        {
            HP = defaultHp;
            ATK = defaultAtk;
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
            yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        public IEnumerator Heal(int amount)
        {
            healAnimator.Play("heal");
            HP += amount;
            yield return new WaitForSeconds(healAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        public IEnumerator TakeDamage(int amount, bool isEventDamage)
        {
            if (isEventDamage)
            {
                boomAnimator.Play("boom");
                yield return new WaitForSeconds(boomAnimator.GetCurrentAnimatorStateInfo(0).length);
            }

            if (IsDefend)
            {
                HP -= amount / 2;
                IsDefend = false;
            }
            else
            {
                HP -= amount;
            }

            characterAnimator.Play("injure");
            yield return new WaitForSeconds(characterAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        public int GetMaxHp()
        {
            return defaultHp;
        }

        private int LoadValueFromFile(string fileName)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "PlayerConfig", fileName + ".csv");

            if (!File.Exists(filePath))
            {
                Debug.LogError($"File {fileName} not found!");
                return ReturnDefaultFileValue(fileName);
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
            return ReturnDefaultFileValue(fileName);
        }

        private int ReturnDefaultFileValue(string type)
        {
            return type == "atk" ? 10 : 100;
        }
    }
}
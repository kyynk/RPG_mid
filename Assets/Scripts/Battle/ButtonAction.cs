using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPGBattle
{
    public class ButtonAction : MonoBehaviour
    {
        public BattleSystem battleSystem;
        public Button attackButton;
        public Button defendButton;
        public Button continueButton;

        void Start()
        {
            // Add listener for mouse clicks
            attackButton.onClick.AddListener(() => StartCoroutine(Attack()));
            defendButton.onClick.AddListener(() => StartCoroutine(Defend()));
            continueButton.onClick.AddListener(ContinueNextMatch);

            // Initially disable ContinueButton
            continueButton.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (attackButton.interactable && defendButton.interactable)
            {
                if (Input.GetKeyDown("a"))
                {
                    StartCoroutine(Attack());
                }
                else if (Input.GetKeyDown("d"))
                {
                    StartCoroutine(Defend());
                }
            }
            else if (Input.GetKeyDown("c"))
            {
                ContinueNextMatch();
            }
        }

        public IEnumerator Attack()
        {
            yield return battleSystem.NextTurn("attack");
        }

        public IEnumerator Defend()
        {
            yield return battleSystem.NextTurn("defend");
        }

        public void DisableFighterActionButtons()
        {
            attackButton.interactable = false;
            defendButton.interactable = false;
        }

        public void EnableFighterActionButtons()
        {
            attackButton.interactable = true;
            defendButton.interactable = true;
        }

        public void EnableContinueButton()
        {
            continueButton.gameObject.SetActive(true); // Show Continue button
        }

        public void DisableContinueButton()
        {
            continueButton.gameObject.SetActive(false); // Hide Continue button
        }

        private void ContinueNextMatch()
        {
            battleSystem.ContinueToNextMatch();
        }
    }
}

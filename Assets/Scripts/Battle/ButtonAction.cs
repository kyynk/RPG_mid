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
            attackButton.onClick.AddListener(Attack);
            defendButton.onClick.AddListener(Defend);
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
                    Attack();
                }
                else if (Input.GetKeyDown("d"))
                {
                    Defend();
                }
            }
            else if (Input.GetKeyDown("c"))
            {
                ContinueNextMatch();
            }
        }

        public void Attack()
        {
            battleSystem.NextTurn("attack");
        }

        public void Defend()
        {
            battleSystem.NextTurn("defend");
        }

        public void DisableFighterActionButtons()
        {
            attackButton.interactable = false;
            defendButton.interactable = false;
            continueButton.gameObject.SetActive(true); // Show Continue button
        }

        public void EnableFighterActionButtons()
        {
            attackButton.interactable = true;
            defendButton.interactable = true;
            continueButton.gameObject.SetActive(false); // Hide Continue button
        }

        private void ContinueNextMatch()
        {
            battleSystem.ContinueToNextMatch();
        }
    }
}

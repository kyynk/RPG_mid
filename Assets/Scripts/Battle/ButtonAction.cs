using UnityEngine;
using UnityEngine.UI;

namespace RPGBattle
{
    public class ButtonAction : MonoBehaviour
    {
        public BattleSystem battleSystem;
        public Button attackButton;
        public Button defenceButton;
        public Button continueButton;

        void Start()
        {
            // Add listener for mouse clicks
            attackButton.onClick.AddListener(Attack);
            defenceButton.onClick.AddListener(Defence);
            continueButton.onClick.AddListener(ContinueNextMatch);

            // Initially disable ContinueButton
            continueButton.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (attackButton.interactable && defenceButton.interactable)
            {
                if (Input.GetKeyDown("a"))
                {
                    Attack();
                }
                else if (Input.GetKeyDown("d"))
                {
                    Defence();
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

        public void Defence()
        {
            battleSystem.NextTurn("defence");
        }

        public void DisableFighterActionButtons()
        {
            attackButton.interactable = false;
            defenceButton.interactable = false;
            continueButton.gameObject.SetActive(true); // Show Continue button
        }

        public void EnableFighterActionButtons()
        {
            attackButton.interactable = true;
            defenceButton.interactable = true;
            continueButton.gameObject.SetActive(false); // Hide Continue button
        }

        private void ContinueNextMatch()
        {
            battleSystem.ContinueToNextMatch();
        }
    }
}

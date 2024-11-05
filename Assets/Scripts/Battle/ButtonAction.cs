using UnityEngine;
using UnityEngine.UI;

namespace RPGBattle
{
    public class ButtonAction : MonoBehaviour
    {
        public BattleSystem battleSystem;
        public Button attackButton;
        public Button defenceButton;

        void Start()
        {
            // Add listener for mouse clicks
            attackButton.onClick.AddListener(Attack);
            defenceButton.onClick.AddListener(Defence);
        }

        // Update is called once per frame
        void Update()
        {
            // Trigger Attack with A key or Left Mouse Click
            if (Input.GetKeyDown("a"))
            {
                Attack();
            }

            // Trigger Defence with D key or Left Mouse Click
            if (Input.GetKeyDown("d"))
            {
                Defence();
            }
        }

        public void Attack()
        {
            Debug.Log("Attack triggered!");
            battleSystem.NextTurn("attack");
        }

        public void Defence()
        {
            Debug.Log("Defence triggered!");
            battleSystem.NextTurn("defence");
        }
    }
}

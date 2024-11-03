using UnityEngine;
using UnityEngine.UI;

namespace BattleState
{
    public class FighterAction : MonoBehaviour
    {
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
            // Trigger Attack with Spacebar or Left Mouse Click
            if (Input.GetKey("a"))
            {
                Attack();
            }

            // Trigger Defence with D key or Right Mouse Click
            if (Input.GetKey("d"))
            {
                Defence();
            }
        }

        public void Attack()
        {
            Debug.Log("Attack triggered!");
            // Add attack functionality here
        }

        public void Defence()
        {
            Debug.Log("Defence triggered!");
            // Add defence functionality here
        }
    }
}

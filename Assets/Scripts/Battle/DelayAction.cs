using System;
using UnityEngine;

namespace RPGBattle
{
    public class DelayedAction : MonoBehaviour
    {
        private Action action;

        public void Setup(float delay, Action action)
        {
            this.action = action;
            Invoke(nameof(ExecuteAction), delay);
        }

        private void ExecuteAction()
        {
            action?.Invoke();
            Destroy(gameObject); // Clean up the timer object
        }
    }
}

using UnityEngine;

namespace BattleState
{
    public class EventHandler
    {
        bool isCritical;
        bool isHeal;

        public void OnPlayerAttack(IPlayer player, IPlayer enemy)
        {
            isCritical = Random.value < 0.3f;
            player.Attack(enemy, isCritical);
        }

        public void OnPlayerHeal(IPlayer player)
        {
            isHeal = Random.value < 0.3f;
            if (isHeal)
            {
                int amount = Random.Range(10, 20);
                player.Heal(amount);
            }
        }

        // when player take damage, the amount is random between 10 and 20 (random events)
        public void OnPlayerTakeDamage(IPlayer player)
        {
            int amount = Random.Range(5, 10);
            player.TakeDamage(amount);
        }
    }
}

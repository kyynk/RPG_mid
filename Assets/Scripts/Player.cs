using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;
    public int atk;

    public Player()
    {
        ResetStatus();
    }

    public void ResetStatus()
    {
        hp = 100;
        atk = 10;
    }

    public void Attack(Player enemy, bool isCritical)
    {
        if (isCritical)
        {
            enemy.TakeDamage(atk * 2);
        }
        else
        {
            enemy.TakeDamage(atk);
        }
    }

    public void Heal(int amount)
    {
        hp += amount;
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject healthFill;

    //[Header("Stats")]
    public int health;
    public int attack;
    public bool isDefend;

    private int startHealth;

    private Transform healthTransform;

    private Vector2 healthScale;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

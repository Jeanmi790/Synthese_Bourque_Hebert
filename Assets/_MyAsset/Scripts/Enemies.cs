using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [Header("Attacks")]
    [SerializeField] float attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float radiusAttack;
    

    private Animator enemyAnim = default;
    private Rigidbody2D enemyRb = default;
    private Vector2 movement;
    private Transform target;


    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
         movement = (target.position - transform.position).normalized;   
        }
        
        FlipSprite();
    }  

    void FixedUpdate()
    {
        Walk();
    }

    private void FlipSprite()
    {
        bool flipSprite = movement.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipSprite ? 180f : 0f, 0f));
    }
    private void Walk()
    {
        if (target == null)
        {
            return;
        }

       if (movement != Vector2.zero)
        {
           var xMovement = movement.x * walkSpeed * Time.deltaTime;
           this.transform.Translate(new Vector3(xMovement, 0f), Space.World);
            enemyAnim.SetBool("Walk", true);
        }
        else
        {
            enemyAnim.SetBool("Walk", false);
        }
           
    }

    public void TakingDamage(float dps)
    {
        health -= (int)dps;
        Debug.Log("HP:"+health);
       // enemyAnim.SetBool("Hit", true);
        if (health <= 0)
        {
            Destroy(gameObject);
            //enemyAnim.SetBool("Dead", true);
        }
    }

}
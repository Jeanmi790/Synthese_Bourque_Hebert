using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float distance;
    [Header("Attacks")]
    [SerializeField] float attackPower;
    [SerializeField] float attackSpeed;
    [SerializeField] float[] radiusAttack;
    [SerializeField] Transform[] attackRadius;
    [SerializeField] LayerMask playerLayer;


    private Animator enemyAnim = default;
    private Rigidbody2D enemyRb = default;
    private Vector2 movement;
    private Transform target;
    private static int randomAttack;
    private int attackLenght;
    private float canAttack = -1f;


    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attackLenght = enemyAnim.GetInteger("NumberAttack");
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
        Attack();
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

        if (Vector2.Distance(target.position, transform.position) <= distance)
        {
            enemyAnim.SetBool("Walk", false);
        }
        else if (movement != Vector2.zero)
        {
            var xMovement = movement.x * walkSpeed * Time.deltaTime;
            this.transform.Translate(new Vector3(xMovement, 0f), Space.World);
            enemyAnim.SetBool("Walk", true);
        }

    }

    private void Attack()
    {
        if (Vector2.Distance(target.position, transform.position) <= distance && Time.time > canAttack)
        {
            randomAttack = UnityEngine.Random.Range(0, attackLenght);
            enemyAnim.SetInteger("RandomAttack", randomAttack);
            enemyAnim.SetBool("Attack", true);
            DealDamage(attackPower);
            canAttack = Time.time + attackSpeed;
        }
        else
        {
            enemyAnim.SetBool("Attack", false);
        }
    }

    void DealDamage(float dps)
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, radiusAttack[randomAttack], playerLayer);
        foreach (Collider2D player in hitPlayer)
        {
            Debug.Log("Player hit");
            player.GetComponent<Player>().TakingDamage(dps);
        }
    }

    public void TakingDamage(float dps)
    {
        health -= (int)dps;
        enemyAnim.SetTrigger("Hit");

        Debug.Log("HP:"+health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        enemyAnim.SetBool("Dead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        Debug.Log("Enemy is dead");
    }

    void OnDrawGizmosSelected()
    {
        if (attackRadius == null)
            return;
            
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRadius[randomAttack].position, radiusAttack[randomAttack]);
    }
}
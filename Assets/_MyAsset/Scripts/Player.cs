using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("health")]
    [SerializeField] float health = 10;
    [Header("Moving")]
    [SerializeField] float walkSpeed = 4;
    [SerializeField] float runSpeed = 8;
    [SerializeField] float jump = 5;
    [SerializeField] Transform radiusGroundCheck;
    [SerializeField] float radiusGround = 0.2f;
    [SerializeField] LayerMask groundLayers;
    bool isGrounded;
    [Header("Attacks")]
    [SerializeField] float attackStrenght = 10;
    [SerializeField] float kickStrenght = 5;
    [SerializeField] float radiusAttack = 0.5f;
    [SerializeField] Transform AttackRadius;
    [SerializeField] LayerMask enemiesLayers;
    [SerializeField] float attackSpeed = 2f;

    float nextAttackTime = 0f;

    Animator playerAnim = default;
    float playersize = default;
    Rigidbody2D playerRb = default;





    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playersize = transform.localScale.x;
        playerRb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetButton("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
            else if (Input.GetButton("Fire2") || Input.GetKey(KeyCode.F))
            {
                KickAttack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
    

    }


    void FixedUpdate()
    {
        Moving();
        Sauter();
    }

    void Moving()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(x, 0f);

        float actualWalkSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        playerRb.velocity = direction * actualWalkSpeed;


        if (Input.GetButton("Horizontal"))
        {
            playerAnim.SetBool("Moving", true);
        }
        else
        {
            playerAnim.SetBool("Moving", !true);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnim.SetBool("Run", true);

        }
        else
        {
            playerAnim.SetBool("Run", !true);
        }


        if (x < 0f)
        {
            transform.localScale = new Vector3(-playersize, playersize, playersize);
        }
        else if (x > 0f)
        {
            transform.localScale = new Vector3(playersize, playersize, playersize);

        }



    }

    void DealDamage(float dps)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radiusAttack, enemiesLayers);
        foreach (Collider2D Enemies in hitEnemies)
        {
            Debug.Log("Ennemie hit");
            Enemies.GetComponent<Enemies>().TakingDamage(dps);
        }
    }

    void Attack()
    {
        playerAnim.SetTrigger("Attack");
        DealDamage(attackStrenght);

    }

    void KickAttack()
    {
            playerAnim.SetTrigger("Kick");
            DealDamage(kickStrenght);
    }


    public void TakingDamage(float dps)
    {
        health -= dps;
        playerAnim.SetBool("Dps", true);
        if (health <= 0)
        {
            Destroy(gameObject);
            playerAnim.SetTrigger("Dead");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            //PrendreDegats(1);
        }
    }

    void Sauter()
    {
        isGrounded = Physics2D.OverlapCircle(radiusGroundCheck.position, radiusGround, groundLayers);
        if (Input.GetButton("Jump") && isGrounded)
        {
            playerAnim.SetBool("Jump", true);

            playerRb.velocity = new Vector2(playerRb.velocity.x, jump);
        }
        else
        {
            playerAnim.SetBool("Jump", !true);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (AttackRadius == null && radiusGroundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackRadius.position, radiusAttack);
        Gizmos.DrawWireSphere(radiusGroundCheck.position, radiusGround);
    }
}

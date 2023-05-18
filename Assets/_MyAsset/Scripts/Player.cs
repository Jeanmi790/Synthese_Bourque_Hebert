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
    float playerSize;
    Rigidbody2D playerRb = default;


    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerSize = transform.localScale.x;
        playerRb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        Attack();

    }


    void FixedUpdate()
    {
        Moving();

    }

    private void Moving()
    {
        float x = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapCircle(radiusGroundCheck.position, radiusGround, groundLayers);
        float actualWalkSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector2 direction = new Vector2(x * actualWalkSpeed, playerRb.velocity.y);
        playerRb.velocity = direction;

        playerAnim.SetBool("Moving", Input.GetButton("Horizontal"));
        playerAnim.SetBool("Run", Input.GetKey(KeyCode.LeftShift) );
        playerAnim.SetBool("Jump",  Input.GetButton("Jump") );

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            
            playerRb.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
        }

        transform.localScale = new Vector3(x < 0f ? -playerSize : playerSize, playerSize, playerSize);

    }

    void DealDamage(float dps)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radiusAttack, enemiesLayers);
        foreach (Collider2D Enemies in hitEnemies)
        {
            //Debug.Log("Ennemie hit");
            Enemies.GetComponent<Enemies>().TakingDamage(dps);
        }
    }

    void Attack()
    {
        if (Time.time > nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                SwordAttack();
                nextAttackTime = Time.time + 0.5f / attackSpeed;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                KickAttack();
                nextAttackTime = Time.time + 0.5f / attackSpeed;
            }

        }
    }

    void SwordAttack()
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

    void Jump()
    {


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

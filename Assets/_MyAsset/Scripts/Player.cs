using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("health")]
    [SerializeField] private int maxHealth;

    [SerializeField] private HealthBar healthBar;

    [Header("Moving")]
    [SerializeField] private float walkSpeed = 4;

    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float jump = 5;
    [SerializeField] private Transform radiusGroundCheck;
    [SerializeField] private float radiusGround = 0.2f;
    [SerializeField] private LayerMask groundLayers;
    private bool isGrounded;

    [Header("Attacks")]
    [SerializeField] private float attackStrenght = 10;

    [SerializeField] private float kickStrenght = 5;
    [SerializeField] private float radiusAttack = 0.5f;
    [SerializeField] private Transform AttackRadius;
    [SerializeField] private LayerMask enemiesLayers;
    [SerializeField] private float attackSpeed = 1f;

    private float nextAttackTime = 0f;
    private int health;
    private Animator playerAnim = default;
    private float playerSize;
    private Rigidbody2D playerRb = default;

    private void Awake()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerSize = transform.localScale.x;
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Attack();
    }

    private void FixedUpdate()
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
        playerAnim.SetBool("Run", Input.GetKey(KeyCode.LeftShift));
        playerAnim.SetBool("Jump", Input.GetButton("Jump"));

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRb.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
        }

        transform.localScale = new Vector3(x < 0f ? -playerSize : playerSize, playerSize, playerSize);
    }

    private void DealDamage(float dps)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackRadius.position, radiusAttack, enemiesLayers);
        foreach (Collider2D Enemies in hitEnemies)
        {
            //Debug.Log("Ennemie hit");
            Enemies.GetComponent<Enemies>().TakingDamage(dps);
        }
    }

    private void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                SwordAttack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                KickAttack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
    }

    private void SwordAttack()
    {
        playerAnim.SetTrigger("Attack");
        DealDamage(attackStrenght);
    }

    private void KickAttack()
    {
        playerAnim.SetTrigger("Kick");
        DealDamage(kickStrenght);
    }

    public void TakingDamage(float dps)
    {
        health -= (int)dps;
        playerAnim.SetTrigger("Hit");
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        playerAnim.SetBool("Dead", true);
        playerRb.gravityScale = 0f;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            //PrendreDegats(1);
        }
    }

    private void Jump()
    {
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackRadius == null && radiusGroundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackRadius.position, radiusAttack);
        Gizmos.DrawWireSphere(radiusGroundCheck.position, radiusGround);
    }
}
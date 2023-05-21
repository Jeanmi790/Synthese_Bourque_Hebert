using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject container = default;
    [Header("health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject blood;

    [Header("Moving")]
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float jump = 5;
    [SerializeField] private Transform radiusGroundCheck;
    [SerializeField] private float radiusGround = 0.2f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private GameObject dustCloud;
    private bool isGrounded;

    [Header("Attacks")]
    [SerializeField] private float attackStrength = 10;

    [SerializeField] private float kickStrenght = 5;
    [SerializeField] private float radiusAttack = 0.5f;
    [SerializeField] private Transform AttackRadius;
    [SerializeField] private LayerMask enemiesLayers;
    [SerializeField] private float attackSpeed = 1f;

    private Animator playerAnim = default;
    private Rigidbody2D playerRb = default;
    private SpawnManager spawnManager;
    private bool isLeft = false;
    private float nextAttackTime = 0f;
    private float playerSize;
    private int health;
    private float initialAttackSpeed;
    private float initialAttackStrength;

    private IGameInfo gameInfo;

    private void Awake()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameInfo = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerSize = transform.localScale.x;
        playerRb = GetComponent<Rigidbody2D>();
        initialAttackSpeed = attackSpeed;
        initialAttackStrength = attackStrength;
    }

    // Update is called once per frame
    private void Update()
    {
        Attack();
        Block();
    }

    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {
        float x = Input.GetAxis("Horizontal");

        if (x > 0)
        {
            isLeft = false;
        }
        else if (x < 0)
        {
            isLeft = true;
        }

        isGrounded = Physics2D.OverlapCircle(radiusGroundCheck.position, radiusGround, groundLayers);

        float actualWalkSpeed = 0f;

        if (!Block())
        {
            actualWalkSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            playerAnim.SetBool("Moving", Input.GetButton("Horizontal"));
            
            playerAnim.SetBool("Run", Input.GetKey(KeyCode.LeftShift));
  
            playerAnim.SetBool("Jump", Input.GetButton("Jump"));
        }

        Vector2 direction = new Vector2(x * actualWalkSpeed, playerRb.velocity.y);
        playerRb.velocity = direction;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRb.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
        }

        if (Input.GetButton("Horizontal") && Input.GetKey(KeyCode.LeftShift))
        {
            GameObject newDustCloud = Instantiate(dustCloud, radiusGroundCheck.position, Quaternion.identity);
            newDustCloud.transform.parent = container.transform;
        }
        
        transform.localScale = new Vector3(isLeft ? -playerSize : playerSize, playerSize, playerSize);
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
        if (Block())
        {
            return;
        }
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButton("Fire1"))
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
        DealDamage(attackStrength);
    }

    private void KickAttack()
    {
        playerAnim.SetTrigger("Kick");
        DealDamage(kickStrenght);
    }

    private bool Block()
    {
        if (Input.GetButton("Fire2"))
        {
            playerAnim.SetBool("Block", true);
            return true;
        }
        else
        {
            playerAnim.SetBool("Block", false);
            return false;
        }
    }

    public void TakingDamage(float dps)
    {
        if (Block())
        {
            dps = dps / 2;
        }

        health -= (int)dps;
        playerAnim.SetTrigger("Hit");
        healthBar.SetHealth(health);

        Instantiate(blood, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        playerAnim.SetBool("Block", false);
        playerAnim.SetBool("Dead", true);
        gameInfo.GameOver();
        playerRb.gravityScale = 0f;
        playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        spawnManager.mortJoueur();
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackRadius == null && radiusGroundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackRadius.position, radiusAttack);
        Gizmos.DrawWireSphere(radiusGroundCheck.position, radiusGround);
    }

    public void HealthPotion(int pourcent)
    {
        int recuperation = (maxHealth * pourcent) / 100;

        if (health + recuperation > maxHealth)
        {
            recuperation = maxHealth - health;
        }

        health += recuperation;
        healthBar.SetHealth(health);
    }

    public void AttackSpeedPotion(float speed)
    {
        attackSpeed = initialAttackSpeed * 2;
        StartCoroutine(AttackSpeedPotionTimer());
    }

    private IEnumerator AttackSpeedPotionTimer()
    {
        yield return new WaitForSeconds(10);
        attackSpeed = initialAttackSpeed;
    }

    public void StrengthPotion(float damage)
    {
        attackStrength = initialAttackStrength * 2;
        StartCoroutine(StrengthPotionTimer());
    }

    private IEnumerator StrengthPotionTimer()
    {
        yield return new WaitForSeconds(10);
        attackStrength = initialAttackStrength;
    }
}
using System.Collections;
using System.Linq.Expressions;
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

    [SerializeField] private float rollSpeed = 8;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float runSpeed = 8;
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

    [Header("Sounds")]
    [SerializeField] private AudioClip[] sounds = default; // 0-attack, 1-hit, 2-block 3-die

    private Animator playerAnim = default;
    private Rigidbody2D playerRb = default;
    private SpawnManager spawnManager;
    private float nextAttackTime = 0f;
    private float playerSize;
    private int health;
    private float initialAttackSpeed;
    private float initialAttackStrength;
    private AudioSource audioSource;

    private IGameInfo gameInfo;

    private void Awake()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameInfo = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerSize = transform.localScale.x;
        playerRb = GetComponent<Rigidbody2D>();
        initialAttackSpeed = attackSpeed;
        initialAttackStrength = attackStrength;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Attack();
        Block();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {
        float x = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(radiusGroundCheck.position, radiusGround, groundLayers);

        float actualWalkSpeed = 0f;

        if (!Block())
        {
            actualWalkSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            playerAnim.SetBool("Moving", Input.GetButton("Horizontal"));

            playerAnim.SetBool("Run", Input.GetKey(KeyCode.LeftShift));
            playerAnim.SetBool("Jump", Input.GetButton("Jump") && isGrounded);
            playerAnim.SetBool("Roll", Input.GetKeyDown(KeyCode.Space));
        }

        Vector2 direction = new Vector2(x * (Input.GetKeyDown(KeyCode.Space) ? rollSpeed : actualWalkSpeed), playerRb.velocity.y);
        playerRb.velocity = Input.GetButton("Jump") && isGrounded ? Vector2.up * jumpForce : direction;

        if (Input.GetButton("Horizontal") && Input.GetKey(KeyCode.LeftShift))
        {
            GameObject newDustCloud = Instantiate(dustCloud, radiusGroundCheck.position, Quaternion.identity);
            newDustCloud.transform.parent = container.transform;
        }
    }

    private void FlipSprite()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(playerSize, playerSize, playerSize);
            healthBar.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-playerSize, playerSize, playerSize);
            healthBar.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void DealDamage(float dps)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackRadius.position, radiusAttack, enemiesLayers);
        foreach (Collider2D Enemies in hitEnemies)
        {
            Enemies.GetComponent<Enemies>().TakingDamage(dps);
        }
    }

    private void Attack()
    {
        if (Block())
        {
            return;
        }

        // Utiliser le temps de l'animation avant d'avoir la prochaine attaque? Animator playerAnim = GetComponent<Animator>();
        //float waitfornextAttack = playerAnim.GetCurrentAnimatorStateInfo(0).length;

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButton("Fire1"))
            {

                AttackType("Attack",attackStrength);
                nextAttackTime = Time.time + 1f / attackSpeed;
                PlaySound(sounds[0], false);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                AttackType("Kick",kickStrenght);
                nextAttackTime = Time.time + 1f / attackSpeed;
                PlaySound(sounds[0], false);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AttackType("AttackB",attackStrength);
                nextAttackTime = Time.time + 1f / attackSpeed;
                PlaySound(sounds[0], false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AttackType("AttackC",attackStrength);
                nextAttackTime = Time.time + 1f / attackSpeed;
                PlaySound(sounds[0], false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AttackType("AttackD",attackStrength);
                nextAttackTime = Time.time + 1f / attackSpeed;
                PlaySound(sounds[0], false);
            }

        }
    }

    private void AttackType(string type,float dps)
    {
        playerAnim.SetTrigger(type);
        DealDamage(dps);
    }
    //private void SwordAttack()
    //{
    //    playerAnim.SetTrigger("Attack");
    //    DealDamage(attackStrength);
    //}

    //private void KickAttack()
    //{
    //    playerAnim.SetTrigger("Kick");
    //    DealDamage(kickStrenght);
    //}

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
        PlaySound(sounds[Block() ? 2 : 1], false);
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
        PlaySound(sounds[3], false);
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

    public void StrengthPotion(float damage)
    {
        attackStrength = initialAttackStrength * 2;
        StartCoroutine(StrengthPotionTimer());
    }

    private IEnumerator AttackSpeedPotionTimer()
    {
        yield return new WaitForSeconds(10);
        attackSpeed = initialAttackSpeed;
    }

    private IEnumerator StrengthPotionTimer()
    {
        yield return new WaitForSeconds(10);
        attackStrength = initialAttackStrength;
    }

    private void PlaySound(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
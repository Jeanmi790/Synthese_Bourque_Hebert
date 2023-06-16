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

    [SerializeField] private float rollSpeed = 8;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private Transform radiusGroundCheck;
    [SerializeField] private float radiusGround = 0.2f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private GameObject dustCloud;
    private bool isGrounded;

    [Header("Attacks")]
    private readonly string[] attackName = { "PlayerAttack", "PlayerKick", "PlayerAttackB", "PlayerAttackC", "PlayerAttackD", "PlayerJumpAttack" };

    private readonly string[] playerHitType = { "PlayerHitA", "PlayerHitB" };

    [SerializeField] private float[] attackStrength = { 10, 5, 7, 12, 20, 15 }; // 0-attack, 1-kick, 2-attackB, 3-attackC, 4-attackD, 5-jumpAttack
    [SerializeField] private float[] attackKnockBack = { 1, 3, 3, 4, 5, 0 }; // 0-attack, 1-kick, 2-attackB, 3-attackC, 4-attackD, 5-jumpAttack
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
        initialAttackStrength = attackStrength[0];
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
            playerAnim.SetBool("Roll", Input.GetKeyDown(KeyCode.Space) && isGrounded);
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

    private void DealDamage(float dps, float impulseForce)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackRadius.position, radiusAttack, enemiesLayers);

        foreach (Collider2D Enemies in hitEnemies)
        {
            Enemies.GetComponent<Enemies>().TakingDamage(dps, impulseForce);
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
                AttackType(attackName[0], attackStrength[0], attackKnockBack[0]);
                PlaySound(sounds[0], false);
            }

            switch (Input.inputString)
            {
                case "f":
                    AttackType(attackName[1], attackStrength[1], attackKnockBack[1]);
                    PlaySound(sounds[0], false);
                    break;

                case "1":
                    AttackType(attackName[2], attackStrength[2], attackKnockBack[2]);
                    PlaySound(sounds[0], false);
                    break;

                case "2":
                    AttackType(attackName[3], attackStrength[3], attackKnockBack[3]);
                    PlaySound(sounds[0], false);
                    break;

                case "3":
                    AttackType(attackName[4], attackStrength[4], attackKnockBack[4]);
                    PlaySound(sounds[0], false);
                    break;
            }

            if (!isGrounded && Input.GetButton("Fire1"))
            {
                AttackType(attackName[5], attackStrength[5], attackKnockBack[5]);
                PlaySound(sounds[0], false);
                playerRb.velocity = Vector2.down * 0.5f;
            }
        }
    }

    private void AttackType(string type, float dps, float impulse)
    {
        playerAnim.Play(type);
        nextAttackTime = Time.time + 1f / attackSpeed;
        DealDamage(dps, impulse);
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
        playerAnim.Play(playerHitType[Random.Range(0, 2)]);
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
        playerAnim.Play("PlayerDead");
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
        attackStrength[0] = initialAttackStrength * 2;
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
        attackStrength[0] = initialAttackStrength;
    }

    private void PlaySound(AudioClip clip, bool loop)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Vies")]
    [SerializeField] float vies = 100;
    [Header("Mouvements")]
    [SerializeField] float vitesse = 4;
    [SerializeField] float vitesseCour = 8;
    [SerializeField] float saut = 5;
    [Header("Attaques")]

    [SerializeField] float attaque = 10;
    [SerializeField] float kick = 5;
    [SerializeField] float radiusAttack = 0.5f;

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
        Attaquer();
    }

    void FixedUpdate()
    {
        Mouvement();
        Sauter();
    }

    void Mouvement()
    {
        float x = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(x, 0f);

        float vitesseActuelle = Input.GetKey(KeyCode.LeftShift) ? vitesseCour : vitesse;

        playerRb.velocity = direction * vitesseActuelle;


        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            playerAnim.SetBool("Mouvements", true);
        }
        else
        {
            playerAnim.SetBool("Mouvements", !true);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerAnim.SetBool("Cour", true);

        }
        else
        {
            playerAnim.SetBool("Cour", !true);
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



    void Attaquer()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnim.SetBool("Attaquer", true);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radiusAttack);
            foreach (Collider2D Ennemie in hitEnemies)
            {
                Ennemie.GetComponent<Ennemie>().PrendreDegats(attaque);
            }
        }
        else
        {
            playerAnim.SetBool("Attaquer", !true);
        }

        if (Input.GetKey(KeyCode.F))
        {
            playerAnim.SetBool("Kick", true);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radiusAttack);
            foreach (Collider2D Ennemie in hitEnemies)
            {
                Ennemie.GetComponent<Ennemie>().PrendreDegats(kick);
            }
        }
        else
        {
            playerAnim.SetBool("Kick", !true);
        }

    }

    public void PrendreDegats(float degats)
    {
        vies -= degats;
        playerAnim.SetBool("Degats", true);
        if (vies <= 0)
        {
            Destroy(gameObject);
            playerAnim.SetBool("Mort", true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ennemi"))
        {
            PrendreDegats(1);
        }
    }

    void Sauter()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetBool("Saut", true);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, saut), ForceMode2D.Impulse);
        }
        else
        {
            playerAnim.SetBool("Saut", !true);
        }
    }
}

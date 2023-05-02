using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int vies = 3;
    [SerializeField] float vitesse = 4;

    Animator playerAnim = default;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Mouvement();
    }

    void Mouvement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, y, 0f);

        transform.Translate(direction * Time.deltaTime * vitesse);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 2), 0f);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            playerAnim.SetBool("Mouvements", true);



        }
 
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                playerAnim.SetBool("Cour", true);
            }
            else
            {
                playerAnim.SetBool("Cour", !true);
            }
        
        //if (x < 0f)
        //{
        //    playerAnim.SetBool("Droite", true);
        //    playerAnim.SetBool("Gauche", !true);
        //}
        //else if (x > 0f)
        //{
        //    playerAnim.SetBool("Droite", !true);
        //    playerAnim.SetBool("Gauche", true);
        //}
        //else
        //{
        //    playerAnim.SetBool("Droite", !true);
        //    playerAnim.SetBool("Gauche", !true);
        //}



    }
}

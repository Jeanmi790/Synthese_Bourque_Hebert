using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemie : MonoBehaviour
{
    [Header("Vies")]
    [SerializeField] float vies = 10;
    [Header("Mouvements")]
    [SerializeField] float vitesse = 4;
    [Header("Attaques")]
    [SerializeField] float attaque = 10;
    [SerializeField] float radiusAttack = 0.5f;

    Animator ennemiAnim = default;
    float ennemisize = default;
    Rigidbody2D ennemiRb = default;

    // Start is called before the first frame update
    void Start()
    {
        ennemiRb = GetComponent<Rigidbody2D>();
        ennemiAnim = GetComponent<Animator>();
        ennemisize = transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Mouvement();
        Attaquer();
    }
    void Mouvement()
    {
        ennemiRb.velocity = new Vector2(-vitesse, ennemiRb.velocity.y);
    }

    void Attaquer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, radiusAttack, LayerMask.GetMask("Player"));
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<Player>().PrendreDegats(attaque);
        }
    }

    public void PrendreDegats(float degats)
    {
        vies -= (int)degats;
        if (vies <= 0)
        {
            Destroy(gameObject);
        }
    }



}

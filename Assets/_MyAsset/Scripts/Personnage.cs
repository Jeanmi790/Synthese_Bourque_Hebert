using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personnage : MonoBehaviour
{
    [Header("Vies")]
    [SerializeField] int vies = 100;
    [Header("Mouvements")]
    [SerializeField] float vitesse = 4;
    [SerializeField] float saut = 5;
    [Header("Attaques")]

    [SerializeField] float attaque = 10;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

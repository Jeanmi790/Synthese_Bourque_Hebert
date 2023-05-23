using UnityEngine;

public abstract class Personnage : MonoBehaviour
{
    protected int vies;
    protected float vitesse;
    protected float attaque;
    protected float radiusAttaque;

    protected Animator persoAnim;
    protected float persoSize;
    protected Rigidbody2D persoRb;

    public Personnage(int vies, float vitesse, float attaque, float radiusAttaque) 
    {
        this.vies = vies;
        this.vitesse = vitesse;
        this.attaque = attaque;
        this.radiusAttaque = radiusAttaque;
        persoSize = transform.localScale.x;
    }


    protected abstract void Mouvement();


    protected abstract void Attaquer();


}

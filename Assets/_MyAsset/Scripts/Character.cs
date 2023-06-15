using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected int maxHealth;
    protected int health;
    protected float walkSpeed;
    protected float attackStrenght;
    protected float attackRadius;

    protected Animator persoAnim;
    protected float persoSize;
    protected Rigidbody2D persoRb;




    protected abstract void Mouvement();


    protected abstract void Attaquer();


}

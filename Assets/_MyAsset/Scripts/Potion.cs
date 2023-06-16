using System.Collections;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int potionID = default;  //  0=HealthPotion   1=AttackSpeedPotion    2=StrengthPotion
    private PotionUI potionUI;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        potionUI = FindObjectOfType<PotionUI>();
        StartCoroutine(DestroyPotion());
    }

    private IEnumerator DestroyPotion()
    {
        yield return new WaitForSeconds(3f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.75f);
        yield return new WaitForSeconds(1f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        yield return new WaitForSeconds(1f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.25f);
        yield return new WaitForSeconds(1f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            switch (potionID)
            {
                case 0:
                    other.GetComponent<Player>().HealthPotion(20); // pourcentage de la vie max du player
                    break;

                case 1:
                    other.GetComponent<Player>().AttackSpeedPotion(2);
                    potionUI.ShowAttackUI();
                    break;

                case 2:
                    other.GetComponent<Player>().StrengthPotion(2);
                    potionUI.ShowStrengthUI();
                    break;
            }
        }
    }
}
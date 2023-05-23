using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int potionID = default;  //  0=HealthPotion   1=AttackSpeedPotion    2=StrengthPotion
    PotionUI potionUI;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        potionUI = FindObjectOfType<PotionUI>();
        StartCoroutine(DestroyPotion());
    }

    IEnumerator DestroyPotion()
    {
        float fadeTime = 4f;
        float alpha = 1f;

        yield return new WaitForSeconds(3f);
        for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
        {
            alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            yield return null;
        }

        Destroy(this.gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int potionID = default;  //  0=HealthPotion   1=AttackSpeedPotion    2=StrengthPotion
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
                    break;
                case 2:
                    other.GetComponent<Player>().StrengthPotion(2);
                    break;
            }
        }
    }
}

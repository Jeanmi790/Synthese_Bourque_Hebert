using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotionUI : MonoBehaviour
{
    private int attackTimer = 0;
    private int strengthTimer = 0;
    private TextMeshProUGUI attackCountDown;
    private TextMeshProUGUI strengthCountDown;
    private GameObject potionUI;
    [SerializeField] private GameObject attackUI;
    [SerializeField] private GameObject strengthUI;

    private void Awake()
    {
        HidePotionUI();
    }

    private void Update()
    {
        if (attackTimer > 0)
        {
            attackCountDown.text = attackTimer.ToString() + " sec";
        }
        if (strengthTimer > 0)
        {
            strengthCountDown.text = strengthTimer.ToString() + " sec";
        }
        
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1f);
        attackTimer--;
        if (attackTimer == 0)
        {
            HidePotionUI();
            attackTimer = 0;
        }
        else
        {
            StartCoroutine(AttackTimer());
        }
    }

    IEnumerator StrengthTimer()
    {
        yield return new WaitForSeconds(1f);
        strengthTimer--;
        if (strengthTimer == 0)
        {
            HidePotionUI();
            strengthTimer = 0;
        }
        else
        {
            StartCoroutine(StrengthTimer());
        }
    }

    public void ShowAttackUI()
    {
        attackUI.SetActive(true);
        attackTimer = 10;
        attackCountDown = attackUI.transform.Find("TxtPotionTime").gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(AttackTimer());
    }

    public void ShowStrengthUI()
    {
        strengthUI.SetActive(true);
        strengthTimer = 10;
        strengthCountDown = strengthUI.transform.Find("TxtPotionTime").gameObject.gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(StrengthTimer());
    }

    public void HidePotionUI()
    {
        attackUI.SetActive(false);
        strengthUI.SetActive(false);
    }
}

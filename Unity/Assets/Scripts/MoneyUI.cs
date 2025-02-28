using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateMoneyUI();
        PlayerStats.Instance.MoneyChanged += UpdateMoneyUI;
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = $"${PlayerStats.Instance.GetBalance():0.00}";
        }
    }

    private void OnEnable()
    {
        PlayerStats.Instance.MoneyChanged += UpdateMoneyUI;
    }

    private void OnDisable()
    {
        PlayerStats.Instance.MoneyChanged -= UpdateMoneyUI;
    }
}

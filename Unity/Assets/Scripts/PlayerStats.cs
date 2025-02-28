using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    const float INITIAL_BALANCE = 100_000f;

    private float currentBalance = INITIAL_BALANCE;


    public delegate void OnMoneyChanged();
    public event OnMoneyChanged MoneyChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        //currentBalance = INITIAL_BALANCE;
    }

    private void Awake()
    {
        if (Instance == null)
        {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
        {
            Destroy(gameObject);
        }
    }

    public float GetBalance() => currentBalance;

    public void Spend(float amt)
    {
        if (currentBalance >= amt)
        {
            currentBalance -= amt;
            MoneyChanged?.Invoke();
        }
    }

    public void AddMoney(float amt)
    {
        currentBalance += amt;
        MoneyChanged?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

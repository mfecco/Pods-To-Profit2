using System.Collections;
using System.Collections.Generic;
using TMPro;      // For TextMeshProUGUI
using UnityEngine.UI;  // For Image
using UnityEngine;

public class StoreItemUI : MonoBehaviour
{
    public StoreItem storeItem;
	public TextMeshProUGUI itemNameText;  // Reference to the item name TextMeshPro
	public Button priceButton;   // Reference to the Button component for the price
	public TextMeshProUGUI priceText;     // Reference to the price TextMeshPro
	public Image itemImage;              // Reference to the Image UI element
										 // Start is called before the first frame update
	void Start()
    {
        if (storeItem != null)
        {
            itemNameText.text = storeItem.itemName;
			priceText.text = "$" + storeItem.price.ToString("F2");
			itemImage.sprite = storeItem.image;
			priceButton.onClick.AddListener(OnPriceButtonClick);
		}
    }

	// Button click handler
	void OnPriceButtonClick()
	{
		Debug.Log("Price Button clicked: " + storeItem.itemName + " - $" + storeItem.price);
		PlayerStats.Instance.Spend(storeItem.price);
		// You can add any logic you want to execute when the price button is clicked
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}

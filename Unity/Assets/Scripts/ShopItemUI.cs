using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ShopItemUI : MonoBehaviour
{
	public InventoryItem inventoryItem;
	public Image image;
	public Button purchaseButton;
	public TextMeshProUGUI priceText;
	public TextMeshProUGUI itemNameText;
	public InventoryManager inventoryRef;

	public int customPurchaseQuantity = 1;
	public Button customPurchaseButton;

	// Start is called before the first frame update
	void Start()
	{
		if (inventoryItem != null)
		{
			image.sprite = inventoryItem.sprite;
			priceText.text = "Buy 1 for $" + inventoryItem.purchasePrice.ToString("F2");
			itemNameText.text = inventoryItem.storeName + "\n(" + inventoryItem.farmType.ToString() + ")";
			image.sprite = inventoryItem.shopSprite;
			purchaseButton.onClick.AddListener(purchaseOne);

			if (customPurchaseButton)
			{
				customPurchaseButton.onClick.AddListener(customPurchase);
			}
		}
	}

	private void purchaseOne()
	{
		Debug.Log("Purchased 1 of " + inventoryItem.objectName + " for " + "$" + inventoryItem.purchasePrice.ToString("F2"));
		inventoryRef.purchaseItem(1, inventoryItem);
	}
	private void customPurchase()
	{
		Debug.Log("Purchased " + customPurchaseQuantity.ToString() + " of " + inventoryItem.objectName + " for " + "$" + (inventoryItem.purchasePrice * customPurchaseQuantity).ToString("F2"));
		inventoryRef.purchaseItem(customPurchaseQuantity, inventoryItem);
	}


	// Update is called once per frame
	void Update()
	{

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

	private Dictionary<int, int> inventory = new Dictionary<int, int>();

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning("Awakening singleton of PlayerStats");
			Destroy(gameObject);
		}
	}

	// Add more of an item to the inventory
	public void AddItem(StoreItem item, int quantity)
	{
		if (inventory.ContainsKey(item.itemId))
		{
			inventory[item.itemId] += quantity;
		}
		else
		{
			inventory[item.itemId] = quantity;
		}
		Debug.Log($"Added {quantity} item {item.itemName}(s) Total: {GetItemQuantity(item)}");
	}

	// TODO: add method to decrease amount of item

	// Get the quantity of an item
	public int GetItemQuantity(StoreItem item)
	{
		return inventory.ContainsKey(item.itemId) ? inventory[item.itemId] : 0;
	}
	
	// Update is called once per frame
	void Update()
    {
        
    }
}

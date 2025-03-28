using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItems")]
public abstract class InventoryItem : ScriptableObject
{
	public enum farmTypes
	{
		Organic,
		Sustainable,
		Conventional
	}

	public GameObject prefab;
	public Sprite sprite;
	public Sprite shopSprite;
	public string objectName;
	public int purchasePrice;
	public farmTypes farmType;
	public string storeName;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoreItem", menuName = "Store/Item")]
public class StoreItem : ScriptableObject
{
	public string itemName;
	public int itemId;
	public float price;
	public Sprite image;
}

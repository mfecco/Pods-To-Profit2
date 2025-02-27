using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedObject", menuName = "Game/SeedObject")]
public class SeedObjectSO : ScriptableObject {
    public enum seedTypes
    {
        Organic,
        Sustainable,
        Conventional
    }

    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
    public int purchasePrice;
    public seedTypes seedType;
}
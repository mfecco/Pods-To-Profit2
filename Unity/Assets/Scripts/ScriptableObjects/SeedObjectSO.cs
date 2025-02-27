using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SeedObject", menuName = "Game/SeedObject")]
public class SeedObjectSO : ScriptableObject {

    public Transform prefab;
    public Sprite sprite;
    public string objectName;
    public int purchasePrice;
    public enum seedType{
        Organic,
        Sustainable,
        Conventional
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PesticideObject", menuName = "Game/PesticideObject")]
public class PesticideObjectSO : ScriptableObject
{
    public enum pesticideTypes{
        Fungicide,
        Insecticide,
        Herbicide

    }

    public enum farmTypes
    {
        Organic,
        Sustainable,
        Conventional
    }

    //public GameObject prefab;
    public Sprite sprite;
    public string objectName;
    public int purchasePrice;
    public pesticideTypes pesticideType;
    public farmTypes farmType;

}


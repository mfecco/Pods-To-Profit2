using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DictionaryClass : MonoBehaviour
{
    [SerializeField]
    private List<KeyValuePair<string, int >> dictionaryList = new List<KeyValuePair<string, int>>()
    {
        new("One", 1),
        new KeyValuePair<string, int>("Two", 2),
        new KeyValuePair<string, int>("Three", 3),
    };
    
}
// maybe create some structure with a stack or que that tracks events, their erroding effects 
//also allows for mitigations options to remove modifiers

using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewYield", menuName = "Game/Yield")]
public class Yield : ScriptableObject
{
    public float cropYield;
    public float modifiedcropYield;
    public List<Modifier> activeModifiers = new List<Modifier>();


    //first ugly attempt at tracking total yield based on active modifiers
    public void updateYield()
    {
        modifiedcropYield = cropYield;

        foreach (var modifier in activeModifiers)
        {
            modifiedcropYield = modifiedcropYield - (modifiedcropYield * modifier.impact);
        }
    
        cropYield = modifiedcropYield;
    }
    //clears modifier list 
    public void clearYield()
    {
        cropYield = 0;
        modifiedcropYield = 0;
        activeModifiers.Clear();
        Debug.Log("Event list cleared in Crop Yield");
    }
}


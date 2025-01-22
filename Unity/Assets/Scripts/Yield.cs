// maybe create some structure with a stack or que that tracks events, their erroding effects 
//also allows for mitigations options to remove modifiers

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewYield", menuName = "Game/Yield")]
public class Yield : ScriptableObject
{
    public float cropYield;
    public float modifiedcropYield;
    public List<Modifier> activeModifiers = new List<Modifier>();
    private List<Modifier> expiredModifiers = new List<Modifier>();

    //first ugly attempt at tracking total yield based on active modifiers
    //needs work
    public void updateYield()
    {
        modifiedcropYield = cropYield;

        foreach (var modifier in activeModifiers)
        {
            if (modifier.activeDuration != 0)
            {
                Debug.Log($"The mod {modifier.eventName} - {modifier.phaseName} - {modifier.activeImpact} has been crunched");
                modifiedcropYield = modifiedcropYield - (modifiedcropYield * modifier.activeImpact);
                modifier.modTick();
            }
            if (modifier.activeDuration == 0)
            {
                expiredModifiers.Add(modifier);
                Debug.Log($"{modifier.eventName} - {modifier.phaseName} has expired");
            }
        }

        foreach (var modifier in expiredModifiers)
        {
            Debug.Log($"{modifier.eventName} - {modifier.phaseName} removed");
            activeModifiers.Remove(modifier);
        }
        expiredModifiers.Clear();
    
        cropYield = modifiedcropYield;
    }

    //this is good for when we implement mitigation options
    public void removeMod(string eventName)
{
    for (int i = activeModifiers.Count - 1; i >= 0; i--)
    {
        if (activeModifiers[i].eventName == eventName)
        {
            activeModifiers.RemoveAt(i);
        }
    }
}


    //faster way to calculate modifier effect on yield.  multiply all the modifiers together i.e. a decrease in 30% is a 0.7 multiplier.
    //if we use this, we might want to rethink way to write out effects in Unity.  If we do negative effects, we can just add 1 to all modifiers and multiply.
    public float calcYield()
    {
        float totalMod = cropYield;
        foreach (var modifier in activeModifiers)
        {
            totalMod = modifier.activeImpact * totalMod;
        }

        return totalMod;
    }
    //clears modifier list 
    public void initYield()
    {
        //this is only for testing
        cropYield = 1000;
        modifiedcropYield = 0;
        activeModifiers.Clear();
        expiredModifiers.Clear();
        Debug.Log("Event list cleared in Crop Yield");
    }

}


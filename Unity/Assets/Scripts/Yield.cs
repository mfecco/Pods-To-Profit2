// maybe create some structure with a stack or que that tracks events, their erroding effects 
//also allows for mitigations options to remove modifiers

using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewYield", menuName = "Game/Yield")]
public class Yield : ScriptableObject
{
    public float cropYield;
    public float modifiedcropYield;
    public List<Event> activeEvents = new List<Event>();
    private List<Event> expiredEvents = new List<Event>();
    

    //this is good for when we implement mitigation options
    public void removeEvent(Event e)
{
    if (activeEvents.Contains(e)){
            activeEvents.Remove(e);
        }
    
}


    //faster way to calculate modifier effect on yield.  multiply all the modifiers together i.e. a decrease in 30% is a 0.7 multiplier.
    //if we use this, we might want to rethink way to write out effects in Unity.  If we do negative effects, we can just add 1 to all modifiers and multiply.
    public float calcYield()
    {
        float totalMod = cropYield;
        foreach (var e in activeEvents)
        {
            totalMod = (e.activeMod.activeImpact + 1) * totalMod;
        }

        return totalMod;
    }

    //clears modifier list 
    public void initYield()
    {
        //this is only for testing
        cropYield = 1000;
        modifiedcropYield = 0;
        activeEvents.Clear();
        expiredEvents.Clear();
        Debug.Log("Event list cleared in Crop Yield");
    }




    public void updateEventYield(){
        {
        modifiedcropYield = cropYield;

        //step through activeEvents list to manage each event's active modifier
        foreach (var e in activeEvents)
        {
            //if modifer has a non zero duration
            if (e.activeMod.activeDuration != 0)
            {
                Debug.Log($"The mod {e.name} - {e.activeMod.phaseName} - {e.activeMod.activeImpact} has been crunched");
                modifiedcropYield = modifiedcropYield - (modifiedcropYield * e.activeMod.activeImpact);
                e.activeMod.modTick();
            }
            if (e.activeMod.activeDuration == 0)
            {
                expiredEvents.Add(e);
                Debug.Log($"{e.name} - {e.activeMod.phaseName} has expired");
            }
        }
        // clear the expiredEvents after they have been removed from activeEvents
        foreach (var e in expiredEvents)
        {
            Debug.Log($"{e.name} - {e.activeMod.phaseName} removed");
            activeEvents.Remove(e);
        }
        expiredEvents.Clear();
    
        cropYield = modifiedcropYield;
    }

    }

}

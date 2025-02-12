using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEvent", menuName = "Game/Event")]  // Makes the asset creation option appear in the Unity Editor

//can be used as a model for all different types of events; weather, positive effects,  
public class Event : ScriptableObject
{

    public string eventName;
    public float probability;
    public string eventDescription;
    public Modifier[] modifiers;
    public Modifier activeMod;
    
    //visual effects?
    //mitigation options?

    //debug check that all info is correct
    public void PrintDetails()
    {
        Debug.Log($"Event occured\n {eventName} - {eventDescription}");
    }

   //apply specific phase modifier for event 
    public Modifier getModifier(TurnPhase currentPhase)
    {
        
        foreach (var modifier in modifiers)
        {
            if (modifier.appliedTurnPhase == currentPhase)
            {
                
                modifier.setActive();
                Debug.Log($"Applying {modifier.phaseName} modifier for {eventName} of {modifier.activeImpact}");
                return modifier;
            }
        }   
        return null;
    }
    public void clearActiveMod(){
        activeMod = null;
    }
}

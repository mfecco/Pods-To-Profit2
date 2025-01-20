using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "NewRandomEvent", menuName = "Game/Event")]  // Makes the asset creation option appear in the Unity Editor
public class Event : ScriptableObject
{

    public string eventName;
    public float probability;
    public string eventDescription;
    public Modifier[] modifiers;
    
    //visual effects?
    //mitigation options?

    //debug check that all info is correct
    public void PrintItemDetails()
    {
        Debug.Log($"Event occured\n {eventName} - {eventDescription}");
    }

   //apply specific phase modifier for event - need to fix to apply to yield object when setup
    public Modifier getModifier(TurnPhase currentPhase)
    {
        
        foreach (var modifier in modifiers)
        {
            if (modifier.appliedTurnPhase == currentPhase)
            {
                
                Debug.Log($"Applying {modifier.phaseName} modifier for {eventName} of {modifier.impact}");
                return modifier;
            }
        }
       
        return null;
    }
}

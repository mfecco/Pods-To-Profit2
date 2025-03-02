using System;
using System.Collections.Generic;
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
    [SerializeField] private Modifier[] modifiers;
    [SerializeField] private Modifier activeMod;
    [SerializeField] private float distribution;
    [SerializeField] private GameObject visualPrefab;
    private List<GameObject> visualInstances = new List<GameObject>();
    
    //visual effects?
    //mitigation options?
    public void spawnVisual(Vector3 position)
    {
        visualInstances.Add(Instantiate(visualPrefab, position, Quaternion.identity));
    }

    public void removeVisual()
    {
        foreach(var visual in visualInstances)
        {
            if (visual!=null)
            {
                Destroy(visual);
            }
        }
        visualInstances.Clear();
    }

    //debug check that all info is correct
    public void PrintDetails()
    {
        Debug.Log($"Event occured\n {eventName} - {eventDescription}");
    }

   //apply specific phase modifier for event 
    private Modifier getModifier(TurnPhase currentPhase)
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

    public void setActiveMod(TurnPhase current)
    {
        activeMod = getModifier(current);
    }
    public void clearActiveMod(){
        activeMod = null;
    }

    public float getDistribution()
    {
        return distribution;
    }

    public float getActiveImpact()
    {
        return activeMod.activeImpact;
    }

    public int getActiveDuration()
    {
        return activeMod.activeDuration;
    }

    public void triggerMod()
    {
        activeMod.modTick();
    }
}

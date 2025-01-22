
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//could be used to handle differnt levels of events.  early game event handleing at same time as end game?  
public class RandomEventHandler : MonoBehaviour
{
    public Event[] randomEvents;
    public TurnManager turnmanager;
    private float randomChance;
    public Yield cropYield;

    void Start()
    {
        //this is needed to make sure scriptable object list is clear when game restarts
        cropYield.initYield();
    }
    // Function to trigger a random event
    public void HandleRandomEvent(TurnPhase current)
    {
        // Randomly select an event from the list
        int randomIndex = Random.Range(0, randomEvents.Count());
        randomChance = Random.Range(0f, 1f);
        
        Debug.Log($"The random index chosen was {randomIndex} and the random chance is {randomChance}");
        
        if (randomChance <= randomEvents[randomIndex].probability)
        {
            randomEvents[randomIndex].PrintDetails();
            Modifier mod = randomEvents[randomIndex].getModifier(current);
            if(mod!=null)
            {
                //needs to fix after decide how to treat repeat events
                //maybe check for mods containing certain aspects or event/phase combo
                if(!cropYield.activeModifiers.Contains(mod))
                {
                    Debug.Log($"{mod.eventName} modifier is added");
                    cropYield.activeModifiers.Add(mod);

                }
            }
        }
        //this will need to be moved in order to allow player to recover from active events before applying full impact?
        //this might be better to exist in a different script.
        cropYield.updateYield();
    }   

}

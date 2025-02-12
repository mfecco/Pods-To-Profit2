
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//could be used to handle differnt levels of events.  early game event handleing at same time as end game? 

public class RandomEventHandler : MonoBehaviour
{
    public Event[] randomEvents;
    public EventCategory[] categories;
    public EventCategory firstYear;
    public TurnManager turnmanager;
    private float randomChance;
    public Yield cropYield;


    void Start()
    {
        //this is needed to make sure scriptable object list is clear when game restarts
        cropYield.initYield();
    }
    

    public void PullEvent(TurnPhase current){
        EventCategory ec = categories[(int)current];

        int randomIndex = Random.Range(0, ec.events.Count());
        randomChance = Random.Range(0f, 1f);

        Debug.Log($"{randomIndex} index chosen from range of 0 - {ec.events.Count()} within the {ec.label} category");
        Debug.Log($"{randomChance} dice roll against probability for {ec.events[randomIndex].name} at {ec.events[randomIndex].probability}");

        if(randomChance <= ec.events[randomIndex].probability){
                if(!cropYield.activeEvents.Contains(ec.events[randomIndex]))
                {
                    ec.events[randomIndex].PrintDetails();
                    Debug.Log($"{ec.events[randomIndex].name} Event is added during the {current.ToString()} Phase for the {ec.label} category");
                    ec.events[randomIndex].activeMod = ec.events[randomIndex].getModifier(current);
                    if(ec.events[randomIndex].activeMod != null){

                        cropYield.activeEvents.Add(ec.events[randomIndex]);
                    }

                }
                else
                    Debug.Log($"{ec.events[randomIndex].name} event was picked from the {current.ToString()} phase for the {ec.label} category, however it was already and active event");
            
        }
        cropYield.updateEventYield();

        float totalValue = cropYield.calcYield();

        foreach (var e in cropYield.activeEvents)
        {
            Debug.Log($"--{e.eventName}");
        }

        Debug.Log($"--------------------{cropYield.cropYield}total acres = ${totalValue * 30}---------------------------");
        
    }

    //This function will allow us to script the order of first year events happening per phase.  We want specific events to happen
    // at each phase to give the player a chance to learn game mechanics and not have difficulty run out of control in first year.
    public void firstYearEvents(TurnPhase current){
        firstYear.events[(int)current].activeMod = firstYear.events[(int)current].getModifier(current);
        if(firstYear.events[(int)current].activeMod != null)
        {
            cropYield.activeEvents.Append(firstYear.events[(int)current]);
        }
        cropYield.updateEventYield();
    }

}

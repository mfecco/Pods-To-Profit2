
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

    

    public void PullEvent(TurnPhase current){
        EventCategory ec = categories[(int)current];

        int randomIndex = Random.Range(0, ec.events.Count());
        randomChance = Random.Range(0f, 1f);


        if(randomChance <= ec.events[randomIndex].probability){
                if(cropYield.addEvent(ec.events[randomIndex], current))
                {
                    ec.events[randomIndex].PrintDetails();
                    Debug.Log($"{ec.events[randomIndex].name} Event is added during the {current.ToString()} Phase for the {ec.label} category");
                }
                else
                    Debug.Log($"{ec.events[randomIndex].name} event was picked from the {current.ToString()} phase for the {ec.label} category, however it was already and active event");
            
        }
        cropYield.updateEvents();



        
    }
}

    //This function will allow us to script the order of first year events happening per phase.  We want specific events to happen
    // at each phase to give the player a chance to learn game mechanics and not have difficulty run out of control in first year.
//     public void firstYearEvents(TurnPhase current){
//         firstYear.events[(int)current].activeMod = firstYear.events[(int)current].getModifier(current);
//         if(firstYear.events[(int)current].activeMod != null)
//         {
//             cropYield.activeEvents.Append(firstYear.events[(int)current]);
//         }
//         cropYield.updateEventYield();
//     }

// }

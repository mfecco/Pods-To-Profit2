// maybe create some structure with a stack or que that tracks events, their erroding effects 
//also allows for mitigations options to remove modifiers

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewYield", menuName = "Game/Yield")]
public class Yield : ScriptableObject
{
    public float cropYield;
    [SerializeField] private List<Event> activeEvents = new List<Event>();
    private List<Event> expiredEvents = new List<Event>();
    [SerializeField] private List<HexCell> hexObjects = new List<HexCell>();
    [SerializeField] private List<HexCell> plants;
    

    //this is good for when we implement mitigation options
    public void removeEvent(Event e)
{
    if (activeEvents.Contains(e)){
            activeEvents.Remove(e);
        }
    
}
    public float calcYield()
    {
        float total =0;
        foreach (var hex in plants)
        {
            total += hex.yield.getBushels();
            
        }
        return total;
    }

    //clears modifier list 
    public void initYield()
    {

        cropYield = 0;
        activeEvents.Clear();
        expiredEvents.Clear();
        plants.Clear();
        hexObjects.Clear();
        Debug.Log("Yields - initYield run, all lists cleared");
        foreach(var obj in GameObject.FindGameObjectsWithTag("HexCell"))
        {
            hexObjects.Add(obj.GetComponent<HexCell>());

        }
        Debug.Log($"There are {hexObjects.Count} hexs in grid");
        
    }


    public bool addEvent(Event e, TurnPhase current)
    {
        if (!activeEvents.Contains(e)){
            e.setActiveMod(current);
            activeEvents.Add(e);
            distributeEvent(e);
            return true;
        }
        else{
            return false;
        }
    }

    private void distributeEvent(Event e)
    {
        float dist = e.getDistribution();
        int affectedPlants = (int)Math.Floor(plants.Count * dist);
        FisherYatesShuffle(plants);
        if (plants.Count>0)
        {
            for (int i =0 ; i<affectedPlants; i++)
            {
                plants[i].yield.addEvent(e);
                Vector3 loc = plants[i].transform.position;
                loc.y += 5;
                if((e.eventName == "Sting Bugs") || (e.eventName=="Three cornered alfalfa hopper"))
                    e.spawnVisual(loc);
            }
        }
    }
//need to find where to call this function. If I call too early, then seeds wont be planted yet and hexObjects is still entire grid. If too late, yields arn't up to date.
    public void findSeeds()
    {
        List<HexCell> temp = new List<HexCell>();

        foreach(var hex in hexObjects)
        {
            if(hex.HasSeedObject())
            {
                if(hex.yield.getBushels()==0)
                    hex.yield.setBushels(60f);
                temp.Add(hex);
                Debug.Log($"{hex} added to temp list with value of {hex.yield.getBushels()}");
            }
            
        }
        Debug.Log($"Temp size in findseeds() is {temp.Count}");
        if(temp.Count > 0)
        {
            plants = temp;
            Debug.Log($"Findseeds() collected {plants.Count} seeds");
        }
    }


    public void updateEvents(){
        
        {
        
        //step through activeEvents list to manage each event's active modifier
        foreach (var e in activeEvents)
        {
            //if modifer has a non zero duration
            if (e.getActiveDuration() != 0)
            {
                e.triggerMod();
                foreach(var hex in plants)
                {
                    if(hex.yield.isEvent(e))
                    {
                        hex.yield.updateYield();
                    }
                }
            }
            if (e.getActiveDuration() == 0)
            {
                expiredEvents.Add(e);
                e.removeVisual();
                foreach(var hex in plants)
                {
                    if(hex.yield.isEvent(e))
                        hex.yield.removeEvent(e);
                }
            }
        }
        // clear the expiredEvents after they have been removed from activeEvents
        foreach (var e in expiredEvents)
        {
            activeEvents.Remove(e);
        }
        expiredEvents.Clear();
    
        cropYield = calcYield();
    }

    }
// using FisherYatesShuffle algorithm
    static void FisherYatesShuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public float seasonEnd()
    {
        float profit = 0f;
        foreach(var plant in plants)
        {
            profit += plant.yield.getBushels();
            plant.ClearSeedObject();
        }

        plants.Clear();
        profit = (float)Math.Truncate(profit*10.25f*100)/100;
        Debug.Log($"Profit for season end is {profit}");
        return profit;
    }


}

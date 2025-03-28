using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantYield : MonoBehaviour
{
    [SerializeField] private List<Event> activeEvents = new List<Event>();
    [SerializeField] private float buschels;




    public void setBushels(float value)
    {
        buschels = value;
    }

    public float getBushels()
    {
        return buschels;
    }

    public bool addEvent(Event e)
    {
        if(!activeEvents.Contains(e))
        {
            activeEvents.Add(e);
            return true;
        }
        return false;
    }

    public bool removeEvent(Event e)
    {
        if(!activeEvents.Contains(e))
        {
            activeEvents.Remove(e);
            return true;
        }
        return false;
    }

    public bool isEvent(Event e)
    {
        return activeEvents.Contains(e);
    }

    public void updateYield(){
        float totalMod = 1f;
        foreach(var e in activeEvents)
        {
            totalMod = totalMod * (e.getActiveImpact() +1);
        }
        setBushels(getBushels()*totalMod);
    }



}
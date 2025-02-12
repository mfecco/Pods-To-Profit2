using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEventCategory", menuName = "Game/EventCategory")]  

public class EventCategory: ScriptableObject
{
    public string label;
    public Event[] events;
    
}


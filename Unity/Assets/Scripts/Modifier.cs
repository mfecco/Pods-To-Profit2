
[System.Serializable]
public class Modifier
{
    public string eventName;
   // Name of the phase modifier assigned to 
    public string phaseName;
    // The effect or impact of the modifier
    public float defaultImpact;
    //could be smaller impact if treated in same phase it arrives.  Might be good to move this for a general var in RandomEvent class        
    public float initialImpact;
    public float activeImpact; 
    //compounding effect if untreated
    public float rateOfChange;
    //amount of turns effect lasts
    public int defaultDuration;
    public int activeDuration;
    //turnphase object for checking
    public TurnPhase appliedTurnPhase;
    
    // can add more properties if needed, like probability, etc.

    //execute a single tick of modifier duration and effect
    //take into account degrading effect over time
    //currently a problem since each object saves updates.
    // need to find a way to reset modifier when it expires.
    public void modTick()
    {
            activeDuration = activeDuration - 1;
            activeImpact = activeImpact - (activeImpact * rateOfChange);
        
    }

}


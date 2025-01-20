
[System.Serializable]
public class Modifier
{
    public string eventName;
   // Name of the phase modifier assigned to 
    public string phaseName;
    // The effect or impact of the modifier
    public float impact;
    //could be smaller impact if treated in same phase it arrives.  Might be good to move this for a general var in RandomEvent class        
    public float initialImpact; 
    //compounding effect if untreated
    public float rateOfChange;
    //amount of turns effect lasts
    public int duration;
    //turnphase object for checking
    public TurnPhase appliedTurnPhase;
    
    // can add more properties if needed, like probability, etc.
}

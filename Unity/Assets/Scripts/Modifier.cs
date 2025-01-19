
[System.Serializable]
public class Modifier
{
    public string eventName;
    public string phaseName; // Name of the phase modifier assigned to
    public float impact;        // The effect or impact of the modifier
    public float rateOfChange; //compounding effect if untreated
    public TurnPhase appliedTurnPhase; //turnphase object for checking
    
    // can add more properties if needed, like duration, probability, etc.
}

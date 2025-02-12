using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* (HP)
 * This script is what controls the stage progress bar (progBar) in the upper right corner 
 * of the UI. More accurately, it moves the object that covers the gradient bar behind it.
 * Each pipe in the graphic below is a location that the progBar jumps to. The pipes outside
 * of the cells are the positions for each of the 6 stages following preplant, which is not
 * shown in the progBar. The pipes inside the cells are the positions for each event that 
 * happens during that stage. The "events" are pretty much just the popups/decisions that the 
 * player has to go through or make during that stage. For example, the first cell is the 
 * Planting stage, which has two events: buying seeds and tilling. So, there are two pipes.
 *
 *             Cotyledon               Fertilizer              Harvest 
 *                 |                       |                      |
 *        /-----\  |  /-----\     /-----\  |  /-----\     /-----\ |
 *       /  | |  \---/   |   \---/ | | | \---/  | |  \---/ | | | \|
 *      |\  | |  /---\   |   /---\ | | | /---\  | |  /---\ | | | /
 *      | \-----/     \-----/  |  \-----/     \-----/  |  \-----/
 *      |                      |                       |
 *   Planting              Vegatative             Reproductive 
 * 
 * Here are the events that I currently have for each stage:
 *  - Planting:     seeds, till
 *  - Cotyledon:    fungicide
 *  - Vegetative:   scout, pesticide, disease
 *  - Fertilizer:   fertilizer, irrigation
 *  - Reproductive: insects, disease, weather
 */


public class stageProgressBar : MonoBehaviour
{
    public TurnManager turnManager;
    public InventoryManager inventoryManager;
    public GameObject progBar;

    /* (HP)
     * The positions[] array is full of the x coordinates for each pipe in the graphic
     * The array is filled in the "Inspector" tab in unity so that the number of events
     * can be easily changed without having to change the code too much. 
     */
    public float[] positions = new float[17];
    int index = 0;
    /* (HP)
     * The stagePositions[] array is essentially the same thing as positions[] but only
     * for the outer pipes (i.e. the x coordinate of each phase)
     */
    float[] stagePositions = new float[6];
    int stageIndex = 0;

    [Header ("Planting Stage Variables")]
    public bool seedBought = false;
    public bool tilled = false;

    [Header ("Cotyledon Stage Variables")]
    public bool fertBought = false;

    [Header ("Vegatative Stage Variables")]
    public bool inscBought = false;
    public bool fungBought = false;

    // Start is called before the first frame update
    void Start()
    {
        stagePositions[0] = 773;        // Planting
        stagePositions[1] = 831;        // Cotyledon
        stagePositions[2] = 901;        // Vegatative
        stagePositions[3] = 971;        // Fertilizer
        stagePositions[4] = 1041;       // Reproductive
        stagePositions[5] = 1111;       // Harvest
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkNextStage(int i) {
        /* (HP)
         * i is the event index of the current phase. It is essentially the index of the pipes
         * inside the cells in the graphic above. 
         */
        switch (turnManager.current) {
            // each case follows the exact same logic as this one
            case TurnPhase.Planting:
                // if the player buys seeds for the first time in Planting phase, advance progBar
                if (i == 0 && !seedBought) {
                    seedBought = true;
                    nextStage();
                // if the player tills for the first time in Planting phase, advance progBar
                } else if (i == 1 && !tilled) {
                    tilled = true;
                    nextStage();
                }
                break;

            case TurnPhase.Cotyledon:
                if (!fertBought) {
                    fertBought = true;
                    nextStage();
                }
                break;

            case TurnPhase.Vegetative:
                if (i == 0 && !inscBought) {
                    inscBought = true;
                    nextStage();
                } else if (i == 1 && !fungBought) {
                    fungBought = true;
                    nextStage();
                }
                break;
            /* (HP)
             * These two phases don't have individual pop-ups implemented for them yet, just 
             * the original screen with all events on it, so just advance the progBar all the
             * way through each event in the respective phase. Assuming nothing changes with
             * the events (the number of them or what they are), these two phases should also
             * follow the exact same logic as those above. 
             */
            case TurnPhase.Fertilizer:
                nextStage();
                nextStage();
                break;
            case TurnPhase.Reproductive:
                nextStage();
                nextStage();
                nextStage();
                break;

            default:
                break;
        }
    }

    // moves the progBar to the next position in the array, cycling back to the start if needed
    public void nextStage() {
        index++;
        if (index == positions.Length) index = 0;
        progBar.transform.localPosition = new Vector3(positions[index], progBar.transform.localPosition.y, 0);
    }

    /* (HP)
     * I haven't implemented them anywhere yet, but the idea behind these was to have a way to 
     * jump to either the next phase or a specific phase without having to click through all the
     * stuff in between -- would be just a dev tool, not an actual feature 
     */
    public void setPhasePosition(int i) {
        progBar.transform.localPosition = new Vector3(stagePositions[index], progBar.transform.localPosition.y, 0);
    }

    public void nextPhasePosition() {
        stageIndex++;
        if (stageIndex == 6) stageIndex = 0;
        progBar.transform.localPosition = new Vector3(stagePositions[stageIndex], progBar.transform.localPosition.y, 0);
    }
}

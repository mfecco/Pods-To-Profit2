using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* (HP)
 * This script is just to control which display is showing in the shop when the user goes to 
 * it. For example, when the user goes to the shop after the first prompt in the planting stage,
 * they need to buy seeds so the seeds display is active, but later the player may return to the 
 * shop after a prompt to buy fungicides and thus the agrochemicals display will be active with
 * the fungicide subdisplay. 
 */
public class shopScreen : MonoBehaviour
{
    /* (HP)
     * display indecies:
     * 0 = all
     * 1 = seeds
     * 2 = fertilizers
     * 3 = field inspection
     * 4 = agrochemicals
     */
    public GameObject[] displayObjects = new GameObject[5];
    public Button[] displayButtons = new Button[5];
    int curIndex = 1;   // current index -- this needs to match the index of the starting display
    /* (HP)
     * subdisplay indecies:
     * 0 = fungicide
     * 1 = insecticide
     * 2 = herbicide
     */
    public GameObject[] subdisplayObjects = new GameObject[3];
    public Button[] subdisplayButtons = new Button[3];
    int curSubIndex = 0;   // current subdisplay index -- this needs to match the index of the starting subdisplay
    
    public Sprite active;
    public Sprite inactive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // the shop buttons call this to update the shop displays and button colors as needed
    public void updateDisplay(int index) {
        // hides the current display and changes the button back to inactive
        displayObjects[curIndex].SetActive(false);
        displayButtons[curIndex].image.sprite = inactive;
        // shows the new display and changes the button to active
        displayObjects[index].SetActive(true);
        displayButtons[index].image.sprite = active;
        // updates the current index so the next call hides the current display appropiately
        curIndex = index;
    }

    public void updateSubdisplay(int index) {
        // hides the current subdisplay
        subdisplayObjects[curSubIndex].SetActive(false);
        // shows the new subdisplay
        subdisplayObjects[index].SetActive(true);
        // updates the current index so the next call hides the current subdisplay appropiately
        curSubIndex = index;
    }
}

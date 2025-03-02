using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* (HP)
 * This script deals with the tool menu in the bottom right corner. The tool menu 
 * "extension" is the upper six hexagons, which slide up and down by clicking on the 
 * cog wheel button. The idea behind the left three hexagons of the extension was to be 
 * a way to show the inventory, so when one is clicked, a "slider" comes out from behind 
 * it and shows the number of each status currently in the inventory. Each slider has 3
 * total slots (one for each status), but only one slot shows at a time unless the player
 * has items of more than one status. For example, if the player only has sustainable
 * seeds, then when the "seeds" button is clicked, only one slot shows with the number
 * of sustainable seeds, but if the player then buys conventional seeds, the slider then
 * moves further out to reveal another slot with the number of conventional seeds.
 *
 * As a side note, I liked the idea of this when I made it at the very beginning of the 
 * semester, but now at the end I don't really see a need for it. If y'all do decide to keep
 * the feature, you will need to expand upon it because as of now there are only three 
 * sliders: pesticides, seeds, and fertilizer. This was all we had at the beginning, but we
 * decided to add back in the different types of pesticides (fungicide, insecticide, and 
 * herbicide), so you'll have to do something to accomodate for these. 
 *
 * On top of the number of sliders no longer matching the number of items in the inventory,
 * there are also a couple unnused slots that just have buttons recycled from the old code
 * but are not actually used for anyting (tool and sell). 
 */
 [System.Serializable] public class SlotData {
    public GameObject slotObject;  // The GameObject that represents the UI slot
    public string itemID;        // A string to hold the item name or other relevant data
}
public class toolMenu : MonoBehaviour
{
    public SlotData[] sliderSlots = new SlotData[9];
    // true means extended, false means collapsed (hidden)
    public bool menuBool = true;    
    public bool[] invBools;
    public GameObject[] sliders;

    public InventoryManager invMan;

    int index = -1; // -1 = none open ; 0 = seed ; 1 = fert ; 2 = pest
    public float moveSpeed = 500;   // simply how fast the tool menu moves up/down

    // The three x values for the sliders (pest, seed, & fert)

    int[] showNumPos = new int[3] {875, 735, 595};
    int pos;
    int showOne = 875;
    int showTwo = 735;
    int showThree = 595;

    /* (HP)
     * I have no idea how I got the coordinates for upPos and downPos, but when using 
     * transform.position = upPos or = downPos, these are the coordinates that made them
     * match the coordinates they actually needed to be, which are upPos2 and downPos2.
     * Since I implemented the transition in between the two positions, as seen in Update(),
     * these coordinates became obsolete, but for some reason I feel like I will need them 
     * later, potentially when I add the "pages" feateure to the tool menu, so here they are.
     * Side note, my idea is that one set of coordinates are local and the other global, or 
     * with respect to the parent, but I'm not sure and don't see a reason to figure it out.
     */

    // public Vector3 upPos = new Vector3(985, 248, 0);
    // public Vector3 downPos = new Vector3(985, -350, 0);
    // public Vector3 upPos2 = new Vector3(25, -290, 0);
    // public Vector3 downPos2 = new Vector3(25, -600, 0);

    /*
     * 0 - 2 = seed slots 1 - 3
     * 3 - 5 = fert slots 1 - 3
     * 6 - 8 = pest slots 1 - 3
     */
    // public GameObject[] sliderSlots = new GameObject[9];

    /* (HP)
     * I don't yet have the actual sprites for the three choices of seeds, so when you get
     * them you will need to attach them here as well as putting them in the seeds display
     * in the shop screen. The code using these is already written and working
     */
    public Sprite[] sliderSprites = new Sprite[5];
   

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        MoveMenu();

        // moves the indexed slider out/in depending on its bool 
        if (index > -1 && invBools[index] == false) {
            closeSliders(index);
        } else if (index > -1 && invBools[index] == true) {
            openSlider(index);
            // Debug.Log("OPEN " + index);
        }

        // updates the images and numbers of each slot as needed
        if (index > -1 && invMan.inventory[index][invMan.inventory[index].Length - 1] > 0)
        {
            int i = 0; // the while() loops until i is the index of a status that is > 0
            int slotsToShow = invMan.inventory[index][invMan.inventory[index].Length - 1];
            // Debug.Log("SLOTS OPEN: " + slotsToShow);
            // Case: Two slots showing
            if (slotsToShow == 2)
            {

                // Show the first slot
                while (i < 3 && invMan.inventory[index][i] == 0) i++; // Find the first non-zero slot
                if (i < 3) 
                {
                    sliderSlots[index * 3].slotObject.GetComponentsInChildren<Image>()[1].enabled = true;
                    sliderSlots[index * 3].slotObject.GetComponentInChildren<TMP_Text>().text = "x" + invMan.inventory[index][i];
                    sliderSlots[index * 3].slotObject.GetComponentsInChildren<Image>()[1].sprite = sliderSprites[(index * 3) + i];
                    sliderSlots[index * 3].itemID = index.ToString() + (((index * 3) + i) % 3).ToString() + (i == 0 ? "0" : "1");
                    i++; // Move to the next item
                }
                // Show the second slot
                while (i < 3 && invMan.inventory[index][i] == 0) i++; // Find the second non-zero slot
                if (i < 3)
                {
                    sliderSlots[index * 3 + 1].slotObject.GetComponentsInChildren<Image>()[1].enabled = true;
                    sliderSlots[(index * 3) + 1].slotObject.GetComponentInChildren<TMP_Text>().text = "x" + invMan.inventory[index][i];
                    sliderSlots[(index * 3) + 1].slotObject.GetComponentsInChildren<Image>()[1].sprite = sliderSprites[(index * 3) + i];
                    sliderSlots[index * 3 + 1].itemID = index.ToString() + ((index * 3 + i) % 3).ToString() + (i == 0 ? "0" : "1");
                }
            }
            else if (slotsToShow == 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    sliderSlots[index * 3 + j].slotObject.GetComponentsInChildren<Image>()[1].enabled = true;
                    sliderSlots[(index * 3) + j].slotObject.GetComponentsInChildren<Image>()[1].sprite = sliderSprites[(index * 3) + j];
                    sliderSlots[(index * 3) + j].slotObject.GetComponentInChildren<TMP_Text>().text = "x" + invMan.inventory[index][j];
                    sliderSlots[index * 3 + j].itemID = index.ToString() + ((index * 3 + j) % 3).ToString() + (j == 0 ? "0" : "1");
                }
                

                // Case: One slot showing
            }
            else
            {
                while (invMan.inventory[index][i] == 0) i++;
                sliderSlots[index * 3].slotObject.GetComponentsInChildren<Image>()[1].enabled = true;
                sliderSlots[index * 3].slotObject.GetComponentsInChildren<Image>()[1].sprite = sliderSprites[(index * 3) + i];
                sliderSlots[index * 3].slotObject.GetComponentInChildren<TMP_Text>().text = "x" + invMan.inventory[index][i];
                sliderSlots[index * 3].itemID = index.ToString() + ((index * 3 + i) % 3).ToString() + (i == 0 ? "0" : "1");
            }

        }
        else
        {
            // Case: 0 slots showing - Disable UI elements
            if (index > -1) {
            for (int j = 0; j < 3; j++)
            {
                sliderSlots[(index * 3) + j].slotObject.GetComponentsInChildren<Image>()[1].enabled = false;
                sliderSlots[(index * 3) + j].slotObject.GetComponentInChildren<TMP_Text>().text = "";
                sliderSlots[index * 3 + j].itemID = "";
            }
            }
        }

    }

    // the cog wheel button calls this function, which just toggles the extension up and down
    public void toggleMenu()
    {
        StartCoroutine(CloseSlidersThenToggleMenu());
    }

    private IEnumerator CloseSlidersThenToggleMenu()
    {
        // Set all sliders to false
        for (int i = 0; i < invBools.Length; i++) invBools[i] = false;

        // Wait for sliders to close
        yield return StartCoroutine(CloseSlidersCoroutine(0));

        // Now toggle menuBool after sliders are closed
        menuBool = !menuBool;
    }

    // toggles slider at index, turns other two to false so they don't just reopen when Update() runs again
    public void toggleInventoryBool(int i) {
        invBools[i] = !invBools[i];
        invBools[nextIndex(i)] = false;
        invBools[nextIndex(nextIndex(i))] = false;
        index = i;
    }

    // rotates the index from 0 -> 1 -> 2 -> 0 
    int nextIndex(int i){ 
        return (i + 1) % 3;
    }

    private void MoveMenu()
    {
        float targetY = menuBool ? 248 : -65; // Target position based on menu state
        float step = moveSpeed * Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.MoveTowards(transform.position.y, targetY, step),
            transform.position.z
        );

        // Ensure sliders close only when the menu is fully closed
        if (!menuBool && transform.position.y == targetY)
        {
            invBools[0] = false;
            invBools[1] = false;
            invBools[2] = false;
            index = -1;
        }
    }

    private IEnumerator CloseSlidersCoroutine(int i)
    {
        bool allClosed = false;
        float targetX = 1033;

        while (!allClosed)
        {
            float step = moveSpeed * Time.deltaTime;

            // Move the target slider towards its close position
            sliders[i].transform.localPosition = new Vector3(
                Mathf.MoveTowards(sliders[i].transform.localPosition.x, targetX, step),
                sliders[i].transform.localPosition.y,
                sliders[i].transform.localPosition.z
            );

            // Close the other two sliders
            sliders[nextIndex(i)].transform.localPosition = new Vector3(
                Mathf.MoveTowards(sliders[nextIndex(i)].transform.localPosition.x, targetX, step),
                sliders[nextIndex(i)].transform.localPosition.y,
                sliders[nextIndex(i)].transform.localPosition.z
            );

            sliders[nextIndex(nextIndex(i))].transform.localPosition = new Vector3(
                Mathf.MoveTowards(sliders[nextIndex(nextIndex(i))].transform.localPosition.x, targetX, step),
                sliders[nextIndex(nextIndex(i))].transform.localPosition.y,
                sliders[nextIndex(nextIndex(i))].transform.localPosition.z
            );

            // Check if all sliders are closed
            allClosed = sliders[i].transform.localPosition.x == targetX &&
                        sliders[nextIndex(i)].transform.localPosition.x == targetX &&
                        sliders[nextIndex(nextIndex(i))].transform.localPosition.x == targetX;

            yield return null; // Wait for the next frame before checking again
        }
    }

    private void closeSliders(int i)
    {
        float targetX = 1033;
        float step = moveSpeed * Time.deltaTime; // Consistent movement speed per frame

        sliders[i].transform.localPosition = new Vector3(
            Mathf.MoveTowards(sliders[i].transform.localPosition.x, targetX, step),
            sliders[i].transform.localPosition.y,
            sliders[i].transform.localPosition.z
        );

        sliders[nextIndex(i)].transform.localPosition = new Vector3(
            Mathf.MoveTowards(sliders[nextIndex(i)].transform.localPosition.x, targetX, step),
            sliders[nextIndex(i)].transform.localPosition.y,
            sliders[nextIndex(i)].transform.localPosition.z
        );

        sliders[nextIndex(nextIndex(i))].transform.localPosition = new Vector3(
            Mathf.MoveTowards(sliders[nextIndex(nextIndex(i))].transform.localPosition.x, targetX, step),
            sliders[nextIndex(nextIndex(i))].transform.localPosition.y,
            sliders[nextIndex(nextIndex(i))].transform.localPosition.z
        );
    }

    private void openSlider(int i)
    {
        // Determine the position based on inventory count
        if (invMan.inventory[i][invMan.inventory[i].Length - 1] <= 1)
        {
            pos = showOne;
        }
        else if (invMan.inventory[i][invMan.inventory[i].Length - 1] == 2)
        {
            pos = showTwo;
        }
        else
        {
            pos = showThree;
        }

        // Define movement step
        float step = moveSpeed * Time.deltaTime;

        // Move the target slider towards its final position
        sliders[i].transform.localPosition = new Vector3(
            Mathf.MoveTowards(sliders[i].transform.localPosition.x, pos, step),
            sliders[i].transform.localPosition.y,
            sliders[i].transform.localPosition.z
        );

        // Close the other two sliders if still open
        int next = nextIndex(i);
        int nextNext = nextIndex(next);

        sliders[next].transform.localPosition = new Vector3(
            Mathf.MoveTowards(sliders[next].transform.localPosition.x, 1033, step),
            sliders[next].transform.localPosition.y,
            sliders[next].transform.localPosition.z
        );

        sliders[nextNext].transform.localPosition = new Vector3(
            Mathf.MoveTowards(sliders[nextNext].transform.localPosition.x, 1033, step),
            sliders[nextNext].transform.localPosition.y,
            sliders[nextNext].transform.localPosition.z
        );

    }

    public void OnInventoryButtonClick(int index)
    {
        // Check index bounds
        if (index >= 0 && index < sliderSlots.Length)
        {
            invMan.changeInventory(sliderSlots[index].itemID + "-1");
            Debug.Log("CLICKED " + sliderSlots[index].itemID + "-1");
        }
        else
        {
            Debug.LogError("Invalid index: " + index);
        }
    }
}

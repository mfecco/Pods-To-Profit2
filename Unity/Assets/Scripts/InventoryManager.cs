using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* (HP)
 * This script does exactly what you would think, it manages the inventory... not too much 
 * else to go into. As of right now, the player has to buy things like seeds, fert, etc., but
 * the ammount they buy has no actual affect on the game (i.e., they only need to buy seeds
 * once and then can til as many acres as they want without having to buy more seeds). So, 
 * y'all definitely need to add some more functionality to all of the buyables in the game  
 */
public class InventoryManager : MonoBehaviour
{
    public TurnManager turnManager;

    public float money = 10000;
    // public Text moneyTextDisplay;
    public int rhizobium = 0;
    public int pesticides = 0;
    public int fert = 0;
    public int seeds = 0;

    public int[][] inventory = new int[5][];
    public int[][] shopPrices = new int[5][];
    public int tillingCost = 7;
    public int scoutingCost = 7;

    string savedInput;

    public TMP_Text[] shopInvTexts = new TMP_Text[9];

    public TMP_Text moneyText;
    public TMP_Text shopMoneyText;
    public TMP_Text rText;
    public TMP_Text pText;
    public TMP_Text fText;

    public bool ownTractor = false;
    public bool brokenTractor = false;

    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = "$" + money;
        shopMoneyText.text = "$" + money;

        /* (HP)
         * These are a bit complicated, but I'm gonna do my to explain how they're set up.
         * The first index (row) is always the type of item (the side comments below) and the
         * second index (column) is the nth choice of that item, organized by status (org ->
         * con). The last column of each row is for the sliders in the toolbar, it is just
         * how many slots to display (see the explanation in toolMenu.cs).
         *
         * As a side note, the idea beheind using arrays for these, particularly shopPrices,
         * was to create an easy way to add more crops in future renditions of this game. In
         * theory, all you have to do is copy/paste the array and update the prices for that
         * crop. If you do add more crops, I would make a crop class and make shopPrices[] a
         * member variable, so you can update the functions to just index into the specific
         * class corresponding to whatever crop the player chose to plant that year.
         *
         * Side note number two: I ran out of time, but I planned on making an third array,
         * structured the same way, for the yield percentages that come with each item. For
         * the same reasoning, this array could just be copy/paste/edited and would be in the
         * crop class.
         */

        /*                        1st      2nd     3rd     num                        */
        inventory[0] = new int[4] {0,       0,      0,      0};     // seed
        /*                        1st      2nd     num                        */
        inventory[1] = new int[3] {0,       0,      0};     // fert
        inventory[2] = new int[3] {0,       0,      0};     // fung
        inventory[3] = new int[3] {0,       0,      0};     // insc
        inventory[4] = new int[3] {0,       0,      0};     // herb

        // 0: seed : { organic (o), conventional (s), GMO (s) }
        shopPrices[0] = new int[3] {-75,    -50,    -80};
        // 1: fert : { organic (o), inorganic (c) }
        shopPrices[1] = new int[2] {-40,    -80};
        // 2: fung : { organic (o), inorganic (c) }
        shopPrices[2] = new int[2] {-20,    -10};
        // 3: insc : { organic (o), inorganic (c) }
        shopPrices[3] = new int[2] {-30,    -15};
        // 4: herb : { organic (o), inorganic (c) }
        shopPrices[4] = new int[2] {-50,    -25};
    }

    // Update is called once per frame
    void Update()
    {
        // rText.text = "Rhizobium: " + rhizobium;
        // pText.text = "BioPesticides: " + pesticides;
        // fText.text = "Fertilizer: " + fert;
    }

    /* (HP)
     * Updates money variable and the money texts (both in the top UI bar, just one for in
     * the main screen and another for in the shop screen) as necessary.
     *
     * As a side note/warning, this function was originally used by the old shopButtonFunc()
     * in TurnManager.cs, but we completely changed how the shop and inventory work so I
     * wrote a different function that the shop buttons call, changeInventory(), which is
     * defined below. The original version of this function returned a bool dependant upon
     * whether the player had enough money left to make the purchase. This check is already
     * handled in changeInventory() so I deleted that and made this return void instead.
     * We had not used shopButtonFunc() at all anymore, but I didn't change the return type
     * until the day before our showcase, so this may cause warnings that I did not think of
     * and could not find in the short time I had left. If this function is giving some error,
     * this is the first thing that I would check.
     */
    public void changeMoney(float amount) {
        money += amount;
        moneyText.text = "$" + money;
        shopMoneyText.text = "$" + money;
    }

    /* (HP)
     * The parameter is a string because only functions with one parameter show up in the
     * Unity editor... for whatever reason... and I figured using one string to hold all the
     * needed parameters would be way easier than programming onClick for each button. There
     * very well could be a smarter workaround, but this was the best I could think of.
     *
     * The string holds four total parameters, which are all self explanatory, but I'll
     * define them here anyways:
     * type : index for the row of inventory[] and shopPrices[]
     * choice : index for the column of inventory[] and shopPrices[]
     * sign : add or subtract from inventory[]
     * amount : the amount to be added/subtracted to/from inventory[]
     */
    public void changeInventory(string typeChoiceStatusAmount) {
        /* (HP)
         * if the input string is "!" then this is from a warning popup where the player has
         * clicked the "ok" button, which essentially just reruns this function after the
         * farming status has been downgraded to not flag the error again -- is like this
         * so the player doesn't have to click "buy" again after saying ok to the popup
         */
        if (typeChoiceStatusAmount == "!") {
            typeChoiceStatusAmount = savedInput;
        }
        int type = typeChoiceStatusAmount[0] - '0';
        int choice = typeChoiceStatusAmount[1] - '0';
        int status = typeChoiceStatusAmount[2] - '0';
        char sign = typeChoiceStatusAmount[3];
        int amount = Int32.Parse(typeChoiceStatusAmount.Substring(4));

        /* (HP)
         * If negative amount, also need to have enough in inventory, or if positive amount,
         * then just change inventory as needed and return true, else return false
         */
        if (sign == '+') {
            if (money >= (amount * shopPrices[type][choice])) {
                if (turnManager.farmingStatus < status) {
                    savedInput = typeChoiceStatusAmount;
                    turnManager.GiveShopWarning(status);
                } else {
                    if (type <= 1 && inventory[type][choice] == 0) {
                        inventory[type][inventory[type].Length - 1]++;
                    } else if (type > 1) {
                        shopInvTexts[((type - 2) * 2) + choice].text = "x" + inventory[type][choice] + " in Inventory";
                    }
                    changeMoney(amount * shopPrices[type][choice]);
                    inventory[type][choice] += amount;
                    Debug.Log("t: " + type + " ; c: " + choice + " ; inv: " + inventory[type][choice]);
                }
            } else {
                // error message: "Not enough money"
            }

        } else {
            if (inventory[type][choice] >= amount) {
                inventory[type][choice] -= amount;
            } else {
                // error message: "Not enough of {item} in inventory"
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;
using JetBrains.Annotations;



/* Handles Turn Phases
 * Make appropriate questions appear at the beginning of a phase
 * Make only appropriate tools available for use in the phase (you'll have to make the logic for tools and add them to the shop)
 * Turn info is pulled from the Miro board and my best guesses
 *
 * HexGrid contains a list of every cell.
 * If you need to change a cell's tile graphic, look at HexMapEditor.HandleInput. Adding new textures is explained in AddingTextures.txt
 * If you need to spawn something on a tile(i.e., a plant), look at the commented out HexCell.InstantiateObject
 *
 * Currently the only "tool" is the tiller, and it's just implemented by changing the cell's graphic.
 * Probably should make a new empty Inventory gameobject and a new script to go with it that contains logic for the tools
 * and tracks player money and tools available to player
 * You can add the money to the UI somewhere and you can remove the checkboxes that say Dirt 1, etc. They were just an example of changing the tiles.
 *
 *
 * FUTURE WORK:
 * Add a yield percentage to cells, use it at the end to calculate the actual yield. Currently cheating and just changing the overall yield when cell's should have an individual effect.
 *
 * (HP)
 * This comment is from the group before Kat and I, but I'm ngl everything it says still 
 * rings mostly true. I didn't work on this script much and it's now late May so my memory
 * may be a bit off, but there's quite a lot in this script that we just don't use anymore.
 * Most of the outdated stuff are old variables that are useless because of the new inventory
 * arrays, but even a function or two are obsolete now. I left everything in because I think
 * a lot of the old stuff could be reused once y'all start implementing more tools and the
 * functionality of the buyables that I talk about in the header comment to InventoryManager
 */
public class TurnManager : MonoBehaviour
{
  //added RandomEventHandler and 7 references in activeTurn() by Matt Fecco 
  //this should be only changes made to implement as of now.
    public RandomEventHandler randomEventHandler;
    public Yield cropYield;
    public TurnPhase current;
    // public TMP_Text phaseText;
    public GameObject[] turnPanels;
    public InventoryManager inventory;

    /*
     *
     * (KM) I'm adding some variables I think we need. We need a farmingStatus variable (0, 1, or 2)
     * that we can check to see if a warning should appear for the player. 
     * Preplant variables:
     * These variables will have values 0,1,or 2 corresponding to organic, sustainable, conventional. Note that some options may
       have two choices for sustainable or others; the second choice would be 1.1
     * Note that the tillType2 variable will causes yield changes in the next year/level.It will only have the value 0 or 2.
     * named tillType2 b/c there is an existing tillType variable that we need to look into later.
     * We also need seedType, seedTreatmentType, and fertType variables for preplant.
     */
    public int farmingStatus = 0;
    public TMP_Text statusText;
    public string[] statuses = {"Organic" , "Sustainable" , "Conventional"};
    public Color[] statusColors = new Color[3];
    public float tillType2 = 0;
    public float seedType = 0;
    public float seedTreatmentType = 0;
    public float fertType = 0;
    

    /* We also need GameObject variables for the three possible warnings */
    public GameObject OrgToConvWarning;
    public GameObject OrgToSusWarning;
    public GameObject SusToConWarning;

    public GameObject ShopOrgToConvWarning;
    public GameObject ShopOrgToSusWarning;
    public GameObject ShopSusToConWarning;

    /* Finally we need a variable saying what decision/choice the player is on, making the warning flow easier to connect */
    public string decision = "66";

    /* Also need the decision game objects so we can set them inactive and active */
    public GameObject PreplantDecision0;
    public GameObject PreplantDecision1;
    public GameObject PreplantDecision2;
    public GameObject PreplantDecision3;

    /* TurnPhaseQuestions game objects so we can set them inactive and active in the code */
    /* Actually we will prolly just use nextPhase for this */
    public GameObject PreplantQuestions;
    public GameObject PlantingQuestions;

    /* More game objects for the planting phase */
    // Instruction 2 is second planting phase instusction 
    public GameObject Instruction2;
    // shopButton is button to enter shop which also has Blinker.cs attached to it
    public GameObject ShopButton;
    public GameObject TillButton;

    /* 
     * There are alredy a bunch of weather variables included in the Cotyledon stage,
     * (is this the only stage where we will have weather events?), but there is no
     * overall temperature variable, so I added that here. May change this later
     */
    public int temp = 80;
    public TMP_Text tempText;

    public tempArrow tmpArw;

    public int years = 1;

    public TMP_Text perks;
    public int perksCounter = 1;

    public TMP_Text phaseText;
    public TMP_Text yearText;

    public float yieldPercent = 1.0f;
    public float perPlantSaleAmount = 50.0f;
    float perPlantSaleAmountReset;
    string text = "";

    public GameObject[] plants;

    [Header ("Tool Buttons")]
    public Button sale;
    /* 
     * 0 - 2 = pest slots 1 - 3
     * 3 - 5 = seed slots 1 - 3
     * 6 - 8 = fert slots 1 - 3
     */
    public Button[] invButtons = new Button[9];
    public bool[] invBools = new bool[9];

    [Header ("Tool Amount Labels")]
    public GameObject rtext;
    public GameObject ptext;
    public GameObject ftext;
    /* 0 - organic ; 1 - sustainable ; 2 - conventional */
    public int[] pestAmounts = new int[3];
    public int[] seedAmounts = new int[3];
    public int[] fertAmounts = new int[3];

    [Header ("Preplant Variables")]
    public float perSeedBasePlantPrice = 25.0f; // Represents the labor cost of planting per tile
    float perSeedBasePlantPriceReset;

    public float tractorLeaseAmount = -100.0f;
    public float notractorPriceModifier = 2.0f;

    public float gmoPriceModifier = 1.25f;
    public float gmoYieldModifier = 1.25f;

    public float subsoilPriceModifier = 1.25f;
    public float subsoilYieldModifier = 1.25f;

    public float notillPriceModifier = .8f;
    public float notillYieldModifier = .8f;

    public float rhizoYieldModifier= 1.1f;
    public int rhizoAmount = -50;

    public float biopestYieldModifier = .8f;
    public float biopestInsectModifier = .5f;
    public int biopestAmount = -50;
    public int fertAmount = -50;
    public int seedsAmount = -50;
    public float insectChance = .5f;
    public GameObject[] preplantToggles;

    public bool rhizoing = false;
    public bool pesticiding = false;

    [Header ("Planting Variables")]
    public TMP_Text plantingRandomChanceText;
    public float tractorBreakdownChance = .25f;

    [Header ("Cotyledon Variables")]
    public float weatherChance = .3f;
    public float weatherYieldModifier = .8f;

    public float diseaseChance = .3f;
    public HexCell[] diseasedCells;

    public TMP_Text cotyledonWeatherRandomChanceText;
    public TMP_Text cotyledonDiseaseRandomChanceText;

    [Header ("Vegatative Variables")]
    public TMP_Text vegDiseaseRandomChanceText;
    public TMP_Text vegInsectRandomChanceText;

    [Header ("Fertilizer Variables")]
    public GameObject[] fertilizerToggles;
    public int fertOrgCost = -300;
    public int fertChemCost = -100;
    public float fertOrgYieldModifier = .8f;
    public float fertChemYieldModifier = 1.1f;
    public float finalFertYield = 0f;
    public float fertOrgSaleModifier = 2.0f;

    public float irrOvrCost = -500.0f;
    public float irrFloodCost = -100.0f;
    public float irrOvrYieldModifier = 1.2f;
    public float irrNoYieldModifier = .8f;

    public bool fertilizing = false;

    [Header ("Reproductive Variables")]
    public TMP_Text repDiseaseRandomChanceText;
    public TMP_Text repInsectRandomChanceText;
    public TMP_Text repWeatherRandomChanceText;


    [Header ("Shop Buttons")]
    public GameObject[] shopButtons;
    public GameObject tractorButton;
    public int[] shopPrices;
    public int[] shopAmounts;



    // (KM) adding Cotyledon game object
    public GameObject cotyledon;
    public GameObject vegetative;

    // Start is called before the first frame update
    void Start()
    {
      // tempText.text = temp + "°";
      cropYield.initYield();
      tempText.text = temp.ToString() + "°";
      current = TurnPhase.Preplant;
      tmpArw = GameObject.FindGameObjectWithTag("Arrow").GetComponent<tempArrow>();
      tmpArw.rotateArrow(temp);
      statusText.text = statuses[farmingStatus];
      statusText.color = statusColors[farmingStatus];
      activeTurn();
      phaseText.text = current.ToString();
      perSeedBasePlantPriceReset = perSeedBasePlantPrice;
      perPlantSaleAmountReset = perPlantSaleAmount;
      // fert.interactable = false;
      invButtons[6].interactable = false;
      invButtons[7].interactable = false;
      invButtons[8].interactable = false;
      // sale.interactable = false;

      ftext.SetActive(false);
    }

    void Reset()
    {
      perSeedBasePlantPrice = perSeedBasePlantPriceReset;
      perPlantSaleAmount = perSeedBasePlantPriceReset;
      sale.interactable = false;
      perks.text = "";
      perksCounter = 1;
      if(inventory.ownTractor)
      {
        updatePerks("Tractor");
      }
    }

    void updatePerks(string item)
    {
      perks.text += (perksCounter + " " + item + "\n");
      perksCounter++;
    }

    public void shopButtonFunc(int index)
    {
      switch(current)
      {
        case TurnPhase.Preplant:
          break;
          
        case TurnPhase.Planting:
          switch(index)
          {
            case 0: // rhizobium
              inventory.changeMoney(shopPrices[index]);
              inventory.rhizobium += shopAmounts[index];
              break;
            case 1: // pesticides
              inventory.changeMoney(shopPrices[index]);
              inventory.pesticides += shopAmounts[index];
              break;
            case 2: // seeds
              inventory.changeMoney(shopPrices[index]);
              inventory.seeds += shopAmounts[index];
              Debug.Log(inventory.seeds);
              /* New shop code we might want to move this code elsewhere later */
              if (inventory.seeds >= 10) 
              {
                // disable shopButton's Blinker.cs and make sure the color is back to normal
                ShopButton.GetComponent<Blinker>().enabled = false;
                ShopButton.GetComponent<Image>().color = Color.white;
                // set instruction 2 to active
                Instruction2.SetActive(true);
              }
              break;
            case 3: // fertilizer
              inventory.changeMoney(shopPrices[index]);
              inventory.fert += shopAmounts[index];
              break;
            default:
              break;
          }
          break;

        case TurnPhase.Cotyledon:
          break;

        case TurnPhase.Vegetative:
          break;

        case TurnPhase.Fertilizer:
          switch(index)
          {
            case 0:
              inventory.changeMoney(shopPrices[index]);
              inventory.fert += shopAmounts[index];
              break;
            default:
              break;
          }
          break;

        case TurnPhase.Harvest:
          break;

        default:
          break;
      }
    }


    void activeTurn()
    {
      /* Maybe add a month display to the UI that changes with the turn phases, a turn is a whole season (for now anyway)
         For now, just showing the turn phase
      */
      cropYield.findSeeds();
      phaseText.text = current.ToString();
      if (years==1)
      {
        randomEventHandler.firstYearEvents(current);
      }
      else
        randomEventHandler.PullEvent(current);
      

      switch(current)
      {
        case TurnPhase.Preplant:
        /*
          THINGS FOR THIS PHASE
            Choose whether you want to use a tractor or not.
              If using tractor, do you own it or need to lease
              If leasing, deduct cost
              If not using, higher cost per seed planted in planting phase?

            Decide GMO or not
              GMO higher yield, more expensive seeds?

            Choose tillage type
              No till - less labor (lower costs?), conventional - in the middle, or subsoil - loosens soil to help roots go down (better yield?)
              Seems like subsoil is just the correct option, but I'm not sure what all the effects are, check miro and do research?

            Weathering impact
              Temperature, Water, Cleanup
              I think these need to be modifiers for the yield based on environmental things

            Weeds
              Herbicide or no herbicde
              Affects available items in the shop and maybe cleanup next round?

            Seed treatment
              Rhizobium - if used helps repair nitrogen in the soil, so better yield?
              Bio pesticides - if used, less insects but worse yield?
        */
          if(years != 1)
          {
            Reset();
          }

          if(years == 6)
          {
            turnPanels[7].SetActive(true);
          }
          else
          {
            turnPanels[(int)current].SetActive(true);
          }

          shopAmounts[0] = 10;
          shopPrices[0] = rhizoAmount;
          shopAmounts[1] = 10;
          shopPrices[1] = biopestAmount;
          /* added these here to avoid an array out of bounds error, will change the prices
           * and starting amounts for each later -- also probably going to convert both
           * shop...[] arrays into 2D arrays so account for the three different statuses
           * (conventional, sustainable, and organic).
           */
          shopAmounts[2] = 10;
          shopPrices[2] = fertAmount;
          shopAmounts[3] = 10;
          shopPrices[3] = seedsAmount;

          shopButtons[0].GetComponentsInChildren<TMP_Text>()[0].text = "Rhizobium $" + Math.Abs(rhizoAmount);
          shopButtons[1].GetComponentsInChildren<TMP_Text>()[0].text = "Pesticides $" + Math.Abs(biopestAmount);
          for(int i = 2; i < shopButtons.Length; i++)
          {
            shopButtons[i].SetActive(false);
          }

          // rhizo.interactable = true;
          // pest.interactable = true;
          invButtons[0].interactable = true;
          invButtons[1].interactable = true;
          invButtons[2].interactable = true;
          rtext.SetActive(true);
          ptext.SetActive(true);

          yearText.text = "Year " + years;
          Debug.Log(current);
          
         

          break;

        case TurnPhase.Planting:
        /* OLD COMMENT
          THINGS FOR THIS PHASE
            Planter Type
            Allow player to use tiller to till as per the choice made in preplanting
            Tilling a tile means a plant was planted there

            Random Chance
            Random chance for tractor to break down if owned or for "household issues" (not sure what those would be)
        */
        /* NEW COMMENT (KM)
           Tell player to go buy seeds
           Tell Player to till to plant seeds
        */
          // rhizo.interactable = false;
          // pest.interactable = false;
          invButtons[0].interactable = false;
          invButtons[1].interactable = false;
          invButtons[2].interactable = false;
          rtext.SetActive(false);
          ptext.SetActive(false);

          turnPanels[(int)current].SetActive(true);
          Debug.Log(current);
          
          

          break;

        case TurnPhase.Cotyledon:
          /*
            All planted tiles should show the first level of plant graphic at this stage
            THINGS FOR THIS PHASE
              Random chance of:
                Temperature, water, and wind to cause negative modifiers
                Disease like seed rot or seedling rot to happen or spread.
                (A cell is aware of all of its neighbors. Probably useful for disease spread.
                 It's accessible via cell.neighbors[direction] where direction = NE, E, SE, SW, W, or NW.
                 Check HexCell and HexDirection scripts for more info.)
          */
          cotyledon.SetActive(true);

          //text = "Looks like disease isn't a problem!";

          plants = GameObject.FindGameObjectsWithTag("Plant");
          foreach(GameObject p in plants)
          {
            p.GetComponent<Plant>().stages[0].SetActive(true);
          }

          if(UnityEngine.Random.Range(0f, 1f) >= diseaseChance)
          {
            //destroyPlant
            text ="Oh no, you lost a plant to disease!";
          }
          // cotyledonDiseaseRandomChanceText.text = text;

          text = "The weather looks good!";
          if(UnityEngine.Random.Range(0f, 1f) >= weatherChance)
          {
            string[] weather = {"It's been really hot lately, your crops are experiencing heat stress! This will affect your yield!", "Oh no, you're going through a drought! This will affect your yield!", "Heavy winds are wreaking havoc in your field! This will affect your yield!"};
            text = weather[UnityEngine.Random.Range(0, 3)];
            yieldPercent *= weatherYieldModifier;
          }
          //cotyledonWeatherRandomChanceText.text = text;

          turnPanels[(int)current].SetActive(true);
          Debug.Log(current);


          break;

        case TurnPhase.Vegetative:
          vegetative.SetActive(true);
          plants = GameObject.FindGameObjectsWithTag("Plant");
          foreach(GameObject p in plants)
          {
            p.GetComponent<Plant>().stages[0].SetActive(false);
            p.GetComponent<Plant>().stages[1].SetActive(true);
          }

          /* Weeds not implemented yet */

          turnPanels[(int)current].SetActive(true);
          Debug.Log(current);


          break;

        case TurnPhase.Fertilizer:
          /*
            Plants should show third level of graphic
            THINGS FOR THIS PHASE
              Choice of fertilizer or not.
              Choose whether to irrigate or not.
                Overhead or flooding options. Not sure right now what the difference is.
          */
          foreach(GameObject p in plants)
          {
            p.GetComponent<Plant>().stages[1].SetActive(false);
            p.GetComponent<Plant>().stages[2].SetActive(true);
          }
          // fert.interactable = true;
          invButtons[6].interactable = true;
          invButtons[7].interactable = true;
          invButtons[8].interactable = true;
          ftext.SetActive(true);

          turnPanels[(int)current].SetActive(true);
          Debug.Log(current);
          
          

          break;

        case TurnPhase.Reproductive:
          /*
            Plants should show fourth level of graphic
            THINGS FOR THIS PHASE
              Random chance of:
                Disease and Insects like in vegatative, but less effects
                Temperature, water, wind like in cotyledon
                Household issues like in planting
          */

          foreach(GameObject p in plants)
          {
            p.GetComponent<Plant>().stages[2].SetActive(false);
            p.GetComponent<Plant>().stages[3].SetActive(true);
          }
          turnPanels[(int)current].SetActive(true);
          Debug.Log(current);
          

          break;

        case TurnPhase.Harvest:
          /*
              This is the last phase of the turn, all plants should be gone after this.
          */

          foreach(GameObject p in plants)
          {
            p.GetComponent<Plant>().stages[3].SetActive(false);
            p.GetComponent<Plant>().stages[4].SetActive(true);
          }
          sale.interactable = true;
          turnPanels[(int)current].SetActive(true);
          Debug.Log(current);
          

          break;

        default:
          Debug.Log("Something went wrong, turn phase doesn't exist.");
          break;
      }
    }

    /* Moves to the next phase when the next turn button is pushed
     * Loops back to the start at the end of the phases
     * Probably should add a tracker for how many times we've been through the whole loop. Treat it as years maybe?
     */
    public void nextTurn()
    {
      if(current != TurnPhase.Harvest)
      {
        current++;
      }
      else
      {
        // Matt added this part to remove seedobjects until we find a better solution
        //inventory.money += cropYield.seasonEnd();

        // Matt, I changed your code line based on the unused sellAllPlants() function near the bottom of this script
        // which now updates the money display in the UI  --Nolan
        inventory.changeMoney(cropYield.seasonEnd());

        foreach(GameObject p in plants)
        //Maija - this is where Yield is occuring, Destroy(p) is a bad destroy method
        //I cant find any actual bugs this causes but use the proper tile method instead
        {
          Destroy(p);
        }
        //rest all cells removed here, implement new version when TurnManager
        years++;
        sale.interactable = false;
        current = TurnPhase.Preplant;
      }
      activeTurn();
    }

     /* (KM) The actual new code for handling preplant decisions and warnings */
     /* We need to update tillType2, seedType, seedTreatmentType, or fertType */

     /* We want the player to be able to change these decisions after preplant,
        so choice is used to say which preplant decision gets changed, type is 0, 1, or 2,
        and there is a decimal placed after if there is more than one decision that results in the same status */
  
     public void UpdatePreplant(string choiceType)
     {
      int choice = choiceType[0] - '0';
      float type = choiceType[1] - '0';
      if (choiceType.Length > 2)
      {
        string decimalString = choiceType.Substring(2, 2);
        float decimalFloat = (float) Convert.ToDouble(decimalString);
        type = type + decimalFloat;
      }
      if (choice == 0)
      {
        tillType2 = type;
        PreplantDecision0.SetActive(false);
        PreplantDecision1.SetActive(true);
      }
      if (choice == 1)
      {
        seedType = type;
        PreplantDecision1.SetActive(false);
        PreplantDecision2.SetActive(true);
      }
      if (choice == 2)
      {
        seedTreatmentType = type;
        PreplantDecision2.SetActive(false);
        PreplantDecision3.SetActive(true);
      }
      if (choice == 3)
      {
        fertType = type;
        PreplantDecision3.SetActive(false);
        PreplantQuestions.SetActive(false);
        //PlantingQuestions.SetActive(true);
        nextTurn();

      }
      UpdateFarmingStatus();
      string output = string.Format("Choice: {0} {1}", choice, type);
      Debug.Log(output);
     }

     public void SetChoiceType(string choiceType)
     {
      decision = choiceType;
     }

     public void CallUpdatePreplant()
     {
      UpdatePreplant(decision);
     }

     /* After each decision, we need to update farmingStatus with this function function. */
    public void UpdateFarmingStatus()
    {
      //farmingStatus = status;
      if(tillType2 >= 2 | seedType >= 2 | seedTreatmentType >= 2 | fertType >= 2)
      {
        farmingStatus = 2;
      }
      else if(tillType2 >= 1 | seedType >= 1 | seedTreatmentType >= 1 | fertType >= 1)
      {
        farmingStatus = 1;
      }
      statusText.text = statuses[farmingStatus];
      statusText.color = statusColors[farmingStatus];
    }
    /* Now we need to check the farming status to see if we should make a warning pop up */
    public void GiveWarning(int potentialStatus) 
    {
      // If the potential new status is greater than current farming status set warning to active
      // Remember org = 0, sus = 1, and con = 2 

      // If status is organic and potential status is conventional
      if(farmingStatus == 0 & potentialStatus >= 2)
      {
        OrgToConvWarning.SetActive(true);
      }

      // If status is organic and potential status is sustainable
      else if(farmingStatus == 0 & potentialStatus >= 1 & potentialStatus < 2)
      {
        OrgToSusWarning.SetActive(true);
      }

      // If status is sustainable and potential status is conventional 
      else if(farmingStatus == 1 & potentialStatus >= 2)
      {
        SusToConWarning.SetActive(true);
      }

      else 
      {
        UpdatePreplant(decision);
      }
    }
    /* (KM) End of the code for handling preplant decisions and warnings */

    /*
     * This is literally just a copy of GiveWarning() that Kat wrote, I just needed a
     * seperate function to handle the shop warnings so I didn't have to change 
     * GiveWarning() and add a bunch of stuff elsewhere. If (when) you add warnings 
     * elsewhere, I would get rid of this function and make the necessary changes and 
     * additions so that GiveWarning() checks the stage/screen to know which warning to
     * activate. You need different warnings for each one because the warnings to 
     * different things depending on when they're given, even if in-game they look the 
     * exact same (the buttons call differnt functions). 
     *
     * If you can't tell, I've already thought through exactly how I would do
     * this, so if you want my input, just reach out.
     */
    public void GiveShopWarning(int potentialStatus) 
    {
      // If the potential new status is greater than current farming status set warning to active
      // Remember org = 0, sus = 1, and con = 2 

      // If status is organic and potential status is conventional
      if(farmingStatus == 0 & potentialStatus >= 2)
      {
        ShopOrgToConvWarning.SetActive(true);
      }

      // If status is organic and potential status is sustainable
      else if(farmingStatus == 0 & potentialStatus >= 1 & potentialStatus < 2)
      {
        ShopOrgToSusWarning.SetActive(true);
      }

      // If status is sustainable and potential status is conventional 
      else if(farmingStatus == 1 & potentialStatus >= 2)
      {
        ShopSusToConWarning.SetActive(true);
      }
    }

    /*
     * This just sets the farmingStatus to whatever is fed into it. Right now it's just 
     * used by the shop warnings, but it will probably be needed if you add any other 
     * warnings elsewhere  
     */

    public void setFarmingStatus(int status) {
      farmingStatus = status;
      statusText.text = statuses[farmingStatus];
      statusText.color = statusColors[farmingStatus];
    }

    public void sellAllPlants()
    {
      int count = 0;
      foreach(GameObject p in plants)
      {
        count++;

        //Maija - another bad Destroy method, change later
        Destroy(p);
      }

      float salePrice = perPlantSaleAmount * count * yieldPercent;
      inventory.changeMoney(salePrice);
    }

    //Maija's Scripts - Do not remove
    public TurnPhase getCurrentPhase(){
      return current;
    }
}

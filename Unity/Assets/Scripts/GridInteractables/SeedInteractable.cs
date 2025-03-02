using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class SeedInteractable : TileInteractable
{

    private HexCell selectedTile;
    private string seedID;
    public InventoryManager InventoryManager;

    //Once buttons are functional, remove the SerializeField from selectedSeed
    //it will start as null and will be set by buttons or back to null once none are in inventory
    [SerializeField] private SeedObjectSO selectedSeedObjectSO;
    [SerializeField] private SeedObjectSO[] seedObjects = new SeedObjectSO[3];
    [SerializeField] private TurnManager turnManager;
    private Player player;

    public void setSeedObjectSObyID(string id) {
        seedID = id;
        if (id[0] == '0') { // 0 is number for seeds in id
            selectedSeedObjectSO = seedObjects[id[1] - '0']; // id[1] is the choice player makes (conv, org, GMO)
        }
    }
    public override void Interact(Player player){
        this.player = player;
        interacting = true;

    }

    public override void Cancel(){
        interacting = false;
        player = null;
    }

    public override void HandleInteractions(){
        //if there is a selectedSeed 
        if(selectedSeedObjectSO != null){
            selectedTile = player.GetSelectedTile();
            //check if the tile is tilled and has no seed already planted, then check if the current phase allows for planting
                
            //TODO: !!!!!!!!!! "!selectedTile.getTilled()" is only checked for false for testing, as we cannot yet change tools, make sure the check
            //is actually being made for selectedTile.getTilled() == true once things are functional
            // (RL) InventoryManager.inventory[seedID[0] - '0'][seedID[1] - '0'] >= 1 is a check to see the amount of the seed is >= 1
            if(selectedTile != null && !selectedTile.getTilled() && !selectedTile.HasSeedObject() && turnManager.getCurrentPhase() == TurnPhase.Planting && InventoryManager.inventory[seedID[0] - '0'][seedID[1] - '0'] >= 1){
                SeedObject.SpawnSeedObject(selectedSeedObjectSO, selectedTile);
                InventoryManager.changeInventory(seedID + "-1");
                //DONE (RL): remove seedObject from inventory WITHOUT calling seedObject.Destroy()
                //seedObject.Destroy is used when a seed is removed from tiles
                if (InventoryManager.inventory[seedID[0] - '0'][seedID[1] - '0'] == 0) selectedSeedObjectSO = null;
                //DONE (RL): if there are no more seeds of the same type in inventory, set selectedSeed to null
                //alternatively, set the selectedTool to null (if you do this, remove the selectedSeed != null line on line 14)
            }
        }
    }
    private void Update() {
        if(interacting){
            HandleInteractions();
        }
    }
}


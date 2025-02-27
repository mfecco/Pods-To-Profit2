using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedInteractable : TileInteractable
{

    private HexCell selectedTile;
    private SeedObject selectedSeed;
    [SerializeField] private TurnManager turnManager;

    public override void Interact(Player player){
        //if there is a selectedSeed 
        if(selectedSeed != null){
            selectedTile = player.GetSelectedTile();
            //check if the tile is tilled and has no seed already planted, then check if the current phase allows for planting
            if(selectedTile.getTilled() && !selectedTile.HasSeedObject() && turnManager.getCurrentPhase() == TurnPhase.Planting){
                selectedSeed.SetSeedObjectParent(selectedTile);
                //TODO: remove seedObject from inventory WITHOUT calling seedObject.Destroy()
                //seedObject.Destroy is used when a seed is removed from tiles

                //TODO: if there are no more seeds of the same type in inventory, set selectedSeed to null
                //alternatively, set the selectedTool to null (if you do this, remove the selectedSeed != null line on line 14)
            }
        }
    }
    /*
    private int tillCost = -500; //500 is an arbitrary value

    public override void Interact(Player player){
        selectedTile = player.GetSelectedTile();
        if(!selectedTile.tilled){
            inventoryManager.changeMoney(tillCost);
            selectedTile.tilled = true;
            player.SetTileUV(TILL_UV, selectedTile);
        }
    }
    */
}


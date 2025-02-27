using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedInteractable : TileInteractable
{

    private HexCell selectedTile;

    //Once buttons are functional, remove the SerializeField from selectedSeed
    //it will start as null and will be set by buttons or back to null once none are in inventory
    [SerializeField] private SeedObjectSO selectedSeedObjectSO;
    [SerializeField] private TurnManager turnManager;

    public override void Interact(Player player){
        //if there is a selectedSeed 
        if(selectedSeedObjectSO != null){
            selectedTile = player.GetSelectedTile();
            //check if the tile is tilled and has no seed already planted, then check if the current phase allows for planting
                
            //TODO: !!!!!!!!!! "!selectedTile.getTilled()" is only checked for false for testing, as we cannot yet change tools, make sure the check
            //is actually being made for selectedTile.getTilled() == true once things are functional
            if(!selectedTile.getTilled() && !selectedTile.HasSeedObject() && turnManager.getCurrentPhase() == TurnPhase.Planting){
                SeedObject.SpawnSeedObject(selectedSeedObjectSO, selectedTile);
                //TODO: remove seedObject from inventory WITHOUT calling seedObject.Destroy()
                //seedObject.Destroy is used when a seed is removed from tiles

                //TODO: if there are no more seeds of the same type in inventory, set selectedSeed to null
                //alternatively, set the selectedTool to null (if you do this, remove the selectedSeed != null line on line 14)
            }
        }
    }
}


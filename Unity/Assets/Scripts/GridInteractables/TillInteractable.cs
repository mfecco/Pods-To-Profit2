using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TillInteractable : TileInteractable
{
    public InventoryManager inventoryManager; //this is going to be changed eventually, I do not know where money will be stored so Im using old implementation for now

    private HexCell selectedTile;
    private const int TILL_UV = 3; //index for the tilled texture in the texture array
    
    private int tillCost = -500; //500 is an arbitrary value

    public override void Interact(Player player){
        selectedTile = player.GetSelectedTile();
        if(!selectedTile.getTilled()){
            inventoryManager.changeMoney(tillCost);
            selectedTile.setTilled(true);
            player.SetTileUV(TILL_UV, selectedTile);
        }
    }

}

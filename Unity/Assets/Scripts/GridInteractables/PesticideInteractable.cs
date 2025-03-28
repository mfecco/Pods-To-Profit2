//see SeedInteractable for detailed comments, these functions are mostly similar

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class PesticideInteractable : TileInteractable
{

    private HexCell selectedTile;
    private string pesticideID;
    public InventoryManager InventoryManager;
    

    [SerializeField] private PesticideObjectSO selectedPesticideObjectSO;
    [SerializeField] private PesticideObjectSO[] pesticideObjects = new PesticideObjectSO[3];
    [SerializeField] private TurnManager turnManager;
    private Player player;

    public void setPesticideObjectSObyID(string id) {
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
        if(selectedPesticideObjectSO != null){
            selectedTile = player.GetSelectedTile();
            if(selectedTile != null && !selectedTile.HasSeedObject()){
                
                switch(selectedPesticideObjectSO.pesticideType){
                    case PesticideObjectSO.pesticideTypes.Fungicide:
                        if(!selectedTile.getFungicide()){
                            selectedTile.setFungicide(true);
                            //decrement Fungicide here
                        }
                        break;
                    case PesticideObjectSO.pesticideTypes.Insecticide:
                        if(!selectedTile.getInsecticide()){
                            selectedTile.setInsecticide(true);
                            //decrement Insecticide here
                        }
                        break;
                    case PesticideObjectSO.pesticideTypes.Herbicide:
                        if(!selectedTile.getHerbicide()){
                            selectedTile.setHerbicide(true);
                            //decrement Herbicide here
                        }
                        break;
                    default:
                        Debug.Log("Unkown pesticide type!");
                        break;
                }
            }
        }
    }

    private void Update() {
        if(interacting){
            HandleInteractions();
        }
    }
}


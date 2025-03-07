//see SeedInteractable for detailed comments, these functions are mostly similar

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FertilizerInteractable : TileInteractable
{

    private HexCell selectedTile;
    private string fertilizerID;
    public InventoryManager InventoryManager;

    [SerializeField] private FertilizerObjectSO selectedFertilizerObjectSO;
    [SerializeField] private FertilizerObjectSO[] fertilizerObjects = new FertilizerObjectSO[3];
    [SerializeField] private TurnManager turnManager;
    private Player player;

    public void setFertilizerObjectSObyID(string id) {
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
        if(selectedFertilizerObjectSO != null){
            selectedTile = player.GetSelectedTile();
            if(selectedTile != null && !selectedTile.HasSeedObject() && !selectedTile.getFertilizer()){
                selectedTile.setFertilizer(true);
            }
        }
    }
    private void Update() {
        if(interacting){
            HandleInteractions();
        }
    }
}


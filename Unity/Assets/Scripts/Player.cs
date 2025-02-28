using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Maija - Important to note: Currently, there is nothing else implemented with player
//aside from what tools is selected and event calls that correspond to player action. 
//The "player" itself is likely going to be implemented as an empty game object 
//unless a future iteration calls for a player character.

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask tileLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;

    
    private HexCell selectedTile;
    private TileInteractable selectedTool;
    void Start()
    {
        //Player becomes a listner/subscriber to GameInput's OnInteractAction event
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        //check if game is running once pausing is implemented
        if (selectedTile != null && selectedTool != null){
            selectedTool.Interact(this); //sends player object as argument
        }

    }

    private void Update() {
        HandleInteractions();
    }

    private void HandleInteractions() {
        Vector3 pos = new Vector3(200, 200, 0);
        Ray ray2 = mainCamera.ScreenPointToRay(pos);
        Debug.DrawRay(ray2.origin, ray2.direction * 10, Color.green);
        
        Ray ray = mainCamera.ScreenPointToRay(gameInput.GetMouseVector());
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        // Debug.Log(ray.GetPoint(400));

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, tileLayerMask)){
            // Debug.Log(raycastHit.transform);
            if (raycastHit.transform.TryGetComponent(out HexCell hexCell)) {
                if(hexCell != selectedTile){
                    SetSelectedTile(hexCell);
                }
            } else {
            SetSelectedTile(null);
            }
        } else {
            // Debug.Log("NOTHING");
            SetSelectedTile(null);
        }
    }

    private void SetSelectedTile(HexCell selectedTile) {
        this.selectedTile = selectedTile;
        // Debug.Log(selectedTile);
        /*
        Maija - Below is a good event to keep in mind in case we want tiles to have a selected tile 
        animation or texture when the user hovers over a new tile, such as a bounce or slight glow

        This is not implemented elsewhere so make sure to create the appropriate listeners if implemented

        OnSelectedTileChanged?.Invoke(this, new OnSelectedTileChangedEventArgs {
            selectedTile = selectedTile
        });
        */
    }
}
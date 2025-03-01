using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    
    public event EventHandler<ChangeTileUVEventArgs> ChangeTileUV;
    public class ChangeTileUVEventArgs : EventArgs {
        public int newUV;
        public HexCell selectedTile;
    }
    
    private HexCell selectedTile;
    [SerializeField] private TileInteractable selectedTool; //NOTE: This is only SerializeField for testing, remove once tools are finished

    //Instances will allow proper public event handling (SetUVsRuntime for example)
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    void Start()
    {
        //Player becomes a listner/subscriber to GameInput's OnInteractAction event
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractActionCanceled += GameInput_OnInteractActionCanceled;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        //check if game is running once pausing is implemented
        if (selectedTile != null && selectedTool != null){
            selectedTool.Interact(this); //sends player object as argument
        }

    }

    private void GameInput_OnInteractActionCanceled(object sender, System.EventArgs e){
        //check if game is running once pausing is implemented
        //alternatively, call this function automatically when game is paused
        if (selectedTile != null && selectedTool != null){
            selectedTool.Cancel();
        }
    }
    
    public void SetTileUV(int newUV, HexCell selectedTile){
        ChangeTileUV?.Invoke(this, new ChangeTileUVEventArgs{
            newUV = newUV,
            selectedTile = selectedTile
        });
    }

    private void Update() {
        HandleInteractions();
    }

    private void HandleInteractions() {

        // (RL) If the pointer is over any game object (UI)
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Fire a raycast from the camera in the direction of the mouse
        //On collision with a tile, set that tile to the currently selectedTile
        Ray ray = mainCamera.ScreenPointToRay(gameInput.GetMouseVector());

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, tileLayerMask)){
            if (raycastHit.transform.TryGetComponent(out HexCell hexCell)) {
                if(hexCell != selectedTile){
                    SetSelectedTile(hexCell);
                }
            } else {
            SetSelectedTile(null);
            }
        } else {
            SetSelectedTile(null);
        }
    }

    private void SetSelectedTile(HexCell selectedTile) {
        this.selectedTile = selectedTile;
        /*
        Maija - Below is a good event to keep in mind in case we want tiles to have a selected tile 
        animation or texture when the user hovers over a new tile, such as a bounce or slight glow

        This is not implemented elsewhere so make sure to create the appropriate listeners if implemented

        OnSelectedTileChanged?.Invoke(this, new OnSelectedTileChangedEventArgs {
            selectedTile = selectedTile
        });
        */
    }

    //public so that buttons may access
    public void SetSelectedTool(TileInteractable selectedTool) {
        this.selectedTool = selectedTool;
    }

    public HexCell GetSelectedTile(){
        return selectedTile;
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;

    private void Awake(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //checks if event listeners are null, then invokes OnInteractAction if !null
        OnInteractAction?.Invoke(this, EventArgs.Empty); 
    }

    public Vector2 GetMouseVector() {
            Vector2 mouseVector = playerInputActions.Player.MousePosition.ReadValue<Vector2>();
            //transformation to Vector3 can be removed if game is turned to 2D
            Vector3 mouseVector3 = mouseVector;
            return mouseVector3;
        }

    //Maija - put WASD / Movement info here if a player character is implemented
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractActionCanceled;
    public event EventHandler OnScout;
    private PlayerInputActions playerInputActions;

    private void Awake(){
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Interact.canceled += Interact_canceled;

        //remove this once scout is done via button
        playerInputActions.Player.Scout.performed += Scout_performed;
    }

    //similar to within Scoutable.cs, you will probably remove this when it is changed to a button
    //it will still exist (most likely), but it will be sourced to another script
    //also remove the scout from playerInputActions/the unity new input manager once this change is implemented!
    private void Scout_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnScout?.Invoke(this, EventArgs.Empty); 
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        //checks if event listeners are null, then invokes OnInteractAction if !null
        OnInteractAction?.Invoke(this, EventArgs.Empty); 
    }

    private void Interact_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractActionCanceled?.Invoke(this, EventArgs.Empty); 
    }

    public Vector2 GetMouseVector() {
            Vector2 mouseVector = playerInputActions.Player.MousePosition.ReadValue<Vector2>();
            //transformation to Vector3 can be removed if game is turned to 2D
            Vector3 mouseVector3 = mouseVector;
            return mouseVector3;
        }

    //Maija - put WASD / Movement info here if a player character is implemented
}

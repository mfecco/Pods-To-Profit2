using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileInteractable : MonoBehaviour
{
    //override Interact when a tool inherits this 
    public bool interacting;
    
    public virtual void Interact(Player player){
        Debug.LogError("TileInteractable.Interact();");
    }

    public virtual void Cancel(){
        Debug.LogError("TileInteractable.Cancel();");
    }

    public virtual void HandleInteractions(){
        Debug.LogError("TileInteractable.HandleInteractions();");
    }
}

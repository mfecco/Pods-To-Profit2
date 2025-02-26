using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileInteractable : MonoBehaviour
{
    //override Interact when a tool inherits this 
    
    public virtual void Interact(Player player){
        Debug.LogError("TileInteractable.Interact();");
    }
}

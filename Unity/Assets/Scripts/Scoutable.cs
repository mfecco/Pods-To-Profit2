using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this script to the "Visual" child object in a scoutable object
//items in the "Visual" child object should ONLY contain visual components
//ie. Particle System in the bugs/pests

public class Scoutable : MonoBehaviour
{
    //we only need this to see when "F/Scout" is pressed, remove when functionality changes
    private GameInput gameInput;

    //put the visual game objects that we are enabling or disabling here
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start() {
        //I really dont like grabbing something like this, but its temporary anyways
        gameInput = GameObject.FindObjectOfType<GameInput>(true);

        Hide();
        gameInput.OnScout += GameInput_OnScout;
    }

    //if/when scouting becomes a button, you will likely have to change where this is being listened from!
    //currently it is in GameInput.cs
    private void GameInput_OnScout(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }


}

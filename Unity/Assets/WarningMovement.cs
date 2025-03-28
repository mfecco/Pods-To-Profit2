using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarningMovement : MonoBehaviour
{
    public GameObject WarningHexagon;
    public GameObject WarningBlock;
    public Shaker Shaker;
    public float durationToSet = 0.3f;
    Transform target;
    Transform target2;
    Vector3 initPos;
    Vector3 initPos2;
    public float moveSpeed = 500;
    // Start is called before the first frame update
    void Start()
    {
        // Call the shake function to shake the warning hexagon
        Shaker.Shake(durationToSet);

        //WarningBlock.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        // Slide the hexagon to the left until it reaches desired position
        if (WarningHexagon.transform.localPosition.x > -272) {
            WarningHexagon.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if(WarningHexagon.transform.localPosition.x < -150) {
                WarningBlock.SetActive(true);
            }
        }    

        // Slide the warning message to the right until it reached desired position
        if(WarningBlock.activeInHierarchy) {
        if (WarningBlock.transform.localPosition.x < 210) {
            WarningBlock.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }  
        }  
            
        
    }
}

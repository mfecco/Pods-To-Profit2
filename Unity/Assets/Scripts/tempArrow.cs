using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* (HP)
 * This script is really simple right now because we did not end up implementing weather,
 * but its purpose is to just rotate the arrow in the temperature gauge in the middle in
 * the top UI bar depending on the current temperature of the weather. As it is right now,
 * 0F is parallel pointing to the blue on the left and 100F is parallel pointing to the 
 * red on the right. I'm writing this explanation two months or so after I originally wrote
 * this code, so take that 0 - 100 scale with a grain of salt. When y'all do implement 
 * weather, I would update this range and maybe even add in subranges (one for each color)
 * that correspond to the temperatures for the crops -- if soybeans start losing yield in 
 * temperatures under 40F or over 90F, have the green range from 40F - 90F instead of the 
 * default 33F - 67F scale that it's at right now.
 */
public class tempArrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //rotateArrow();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void rotateArrow(float temp) {
        float angle = 43 - (temp * (float)1.8);
        transform.Rotate(Vector3.forward, angle, Space.Self);
    }
}

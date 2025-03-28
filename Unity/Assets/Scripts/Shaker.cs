using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* (HP)
 * This is what gives the "!" of the warning pop-ups the shaking movement. Kat wrote this 
 * script so I don't know the specifics of how everything works, but at first glance it 
 * looks pretty simple
 */
public class Shaker : MonoBehaviour
{
    Vector3 initPos;
    Transform target;
    public float duration = 0f;
    public float intensity = 1f;
    //float shakeDuration = 0.0
    bool isShaking = false;
    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Transform>();
        initPos = target.localPosition;
    }

    public void Shake(float durationToSet) 
    {
        if (durationToSet > 0) 
        {
            duration += durationToSet;
        }
    } 

    // Update is called once per frame
    void Update()
    {
        if (duration > 0 && !isShaking)
        {
            StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        isShaking = true;
        var startTime = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < startTime + duration)
        {
            var randomPoint = new Vector3(Random.Range(-1f, 1f)*intensity, Random.Range(-1f, 1f)*intensity, initPos.z);
            target.localPosition = randomPoint;
            yield return null;
        }

        duration = 0f;
        target.localPosition = initPos;
        isShaking = false;       
    }
}

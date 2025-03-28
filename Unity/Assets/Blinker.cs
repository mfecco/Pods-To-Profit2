using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Blinker : MonoBehaviour
{
    public Image image;
    public Color startColor = Color.green;
    public Color endColor = Color.white;
    public float speed = 1;


    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        image.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
        
    }
}

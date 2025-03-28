using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class ResetColor : MonoBehaviour
{
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        image = GetComponent<Image>();
        image.color = Color.white;
        
    }
}

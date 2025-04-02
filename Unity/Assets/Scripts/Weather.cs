using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class Weather : MonoBehaviour
{
    /*   
       public string office = "MRX";
       public int gridX = 84;
       public int gridY = 47;
    */

    // Rainfall
    public float p1acc = 12.1f;
    public float p2acc = 12.66f;
    public float p3acc = 5.97f;
    public float p4acc = 11.6f;
    public float p5acc = 5.95f;

    // Daily temperature, 5 phases each lasting 73 days, for weather recap maybe loop through array and count number of days out of range?
    public float[] p1temp = new float[]
    {
    37.0f, 38.5f, 33.0f, 38.0f, 37.5f, 43.0f, 40.0f, 40.5f, 44.5f, 41.5f, 47.5f, 44.5f, 36.5f, 26.5f, 14.5f, 13.0f, 13.0f, 24.5f, 23.5f, 12.0f, 18.0f, 32.5f, 50.0f, 57.0f, 60.0f, 57.0f, 52.0f, 40.5f, 35.5f, 43.5f, 40.5f,
    43.0f, 56.0f, 53.5f, 52.5f, 53.5f, 48.0f, 49.5f, 54.5f, 57.0f, 57.0f, 51.0f, 42.5f, 44.5f, 49.0f, 54.0f, 50.5f, 31.5f, 35.5f, 42.5f, 49.5f, 53.0f, 58.5f, 55.0f, 41.5f, 48.0f, 65.5f, 68.5f, 54.0f, 40.0f
    };
    public float[] p2temp = new float[]
    {
    45.0f, 52.5f, 54.5f, 67.0f, 61.0f, 58.5f, 61.0f, 62.0f, 50.0f, 45.5f, 46.5f, 54.5f, 62.0f, 68.5f, 61.0f, 56.0f, 52.5f, 38.5f, 43.0f, 59.0f, 55.5f, 64.0f, 50.5f, 54.5f, 62.0f, 59.5f, 50.5f, 52.5f, 55.5f, 66.5f, 68.5f,
    74.0f, 63.5f, 51.0f, 49.0f, 43.5f, 50.0f, 58.5f, 66.0f, 61.0f, 65.5f, 62.5f, 57.5f, 61.0f, 71.5f, 73.0f, 76.0f, 73.0f, 76.5f, 65.5f, 57.5f, 53.5f, 52.0f, 59.0f, 66.5f, 60.5f, 69.0f, 75.0f, 73.0f, 73.0f, 71.5f
    };
    public float[] p3temp = new float[]
    {
    72.5f, 74.5f, 74.5f, 71.0f, 74.0f, 74.0f, 73.5f, 75.0f, 74.5f, 62.0f, 65.0f, 65.5f, 65.5f, 68.5f, 68.0f, 71.5f, 69.5f, 72.5f, 75.5f, 77.5f, 78.5f, 76.5f, 76.0f, 72.0f, 77.5f, 73.5f, 75.0f, 74.5f, 70.0f, 68.0f, 71.0f,
    67.5f, 76.5f, 77.5f, 77.5f, 80.0f, 80.0f, 74.5f, 73.5f, 72.0f, 71.5f, 69.0f, 70.5f, 77.5f, 82.0f, 84.5f, 85.5f, 83.5f, 82.5f, 79.5f, 81.0f, 84.0f, 85.5f, 83.0f, 83.0f, 83.5f, 84.5f, 80.5f, 83.0f, 87.5f, 83.5f
    };
    public float[] p4temp = new float[]
    {
    76.0f, 79.5f, 87.5f, 88.5f, 84.5f, 81.0f, 81.0f, 86.5f, 85.0f, 80.0f, 81.0f, 83.5f, 85.5f, 86.5f, 87.5f, 86.5f, 85.0f, 79.0f, 74.5f, 80.0f, 81.5f, 80.5f, 76.5f, 78.5f, 80.5f, 80.0f, 82.5f, 80.0f, 79.5f, 84.5f, 85.0f,
    87.0f, 84.0f, 81.5f, 83.0f, 85.0f, 84.5f, 79.0f, 83.0f, 80.0f, 74.5f, 74.0f, 76.5f, 79.0f, 81.5f, 82.5f, 87.0f, 85.5f, 80.0f, 77.0f, 73.5f, 70.0f, 76.0f, 80.5f, 79.0f, 80.5f, 82.5f, 84.0f, 86.5f, 87.5f, 86.0f, 83.0f
    };
    public float[] p5temp = new float[]
    {
    78.0f, 76.5f, 74.5f, 77.5f, 80.0f, 80.0f, 68.5f, 66.5f, 67.5f, 72.5f, 75.5f, 73.0f, 79.5f, 77.0f, 78.5f, 79.0f, 74.0f, 76.5f, 79.5f, 81.5f, 82.0f, 81.5f, 80.5f, 77.5f, 71.0f, 73.0f, 68.5f, 65.0f, 69.0f, 72.0f,
    74.5f, 68.0f, 71.0f, 74.5f, 76.0f, 76.0f, 76.5f, 67.0f, 64.0f, 64.5f, 65.5f, 65.5f, 66.5f, 74.5f, 76.5f, 77.5f, 72.0f, 72.0f, 72.5f, 72.5f, 73.0f, 73.5f, 73.5f, 74.5f, 74.5f, 75.0f, 75.5f, 75.5f, 76.0f, 76.5f
    };

    // Soybean price
    public float p1price = 12.50f;
    public float p2price = 11.85f;
    public float p3price = 11.20f;
    public float p4price = 10.15f;
    public float p5price = 9.82f; 


    void Start()
    {
        // StartCoroutine(GetWeatherData(office, gridX, gridY));
    }

    /*IEnumerator GetWeatherData(string office, int gridX, int gridY)
    {
        string url = $"https://api.weather.gov/gridpoints/{office}/{gridX},{gridY}/forecast";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            // Process data

            // Pick a day in the middle of the phase and add up accumulated rain
            // Middle Tennessee
            // Mix/Max Temperature

            //https://api.weather.gov/gridpoints/MRX/84,47/forecast,
        }
    }*/
}
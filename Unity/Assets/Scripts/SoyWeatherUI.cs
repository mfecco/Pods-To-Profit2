using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoyWeatherUI : MonoBehaviour
{
    public Weather weather; // Reference to Weather script

    public TurnManager turnManager;

    public TextMeshProUGUI tempText;

    public TextMeshProUGUI rainText;

    public TextMeshProUGUI priceText;

    void Start()
    {
        if (weather != null && tempText != null)
        {
            tempText.SetText(weather.p1temp[0].ToString() + "°");
            rainText.SetText(weather.p1acc.ToString() + '"');
            priceText.SetText('$' + weather.p1price.ToString() + "/bushel");
        }
    }

    private void FixedUpdate()
    {
        if (turnManager.current.ToString() == "Cotyledon")
        {
            tempText.SetText(weather.p2temp[0].ToString() + "°");
            rainText.SetText(weather.p2acc.ToString() + '"');
        }
    }
}

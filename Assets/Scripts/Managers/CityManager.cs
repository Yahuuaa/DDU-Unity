using TMPro;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public static float income;
    
    public GameObject citizenDisplay;

    void Update()
    {
        citizenDisplay.GetComponent<TextMeshProUGUI>().text = "Citizens: " + CitizenManager.Citizens.Count;
    }
}

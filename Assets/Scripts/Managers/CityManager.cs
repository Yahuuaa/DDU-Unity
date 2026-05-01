using TMPro;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    public static float Income;
    public static float Attraction;
    
    public GameObject citizenDisplay;

    void Update()
    {
        citizenDisplay.GetComponent<TextMeshProUGUI>().text = "Citizens: " + CitizenManager.Citizens.Count;
    }

    public static float GetIncome()
    {
        return Income;
    }

    public static float GetAttraction()
    {
        return Attraction;
    }
}

using Library;
using TMPro;
using UnityEngine;
using Utils;

public class CityManager : MonoBehaviour
{
    public static float Money = 100000f;
    public static float Maintanence = 0f;
    public static float Income = 0f;
    public static float Attraction = 100f;
    
    public GameObject citizenDisplay;
    public GameObject moneyDisplay;
    public GameObject attractionDisplay;
    public GameObject dateDisplay;

    private float _dateTimer = 0f;
    private float _dayTime = 3f;
    
    private int _day = 1;
    private int _month = 1;
    private int _year = 2025;

    void Update()
    {
        int workers = 0;
        foreach (Factory building in WorkManager.Workplaces.Values)
        {
            workers += building.GetWorkers().Count;
        }
        citizenDisplay.GetComponent<TextMeshProUGUI>().text = CitizenManager.Citizens.Count + " Indbyggere (" + workers + ")";
        moneyDisplay.GetComponent<TextMeshProUGUI>().text = "$" + NumberFormat.Format(Money) + " (" + NumberFormat.Format(Income - Maintanence) + ")";
        attractionDisplay.GetComponent<TextMeshProUGUI>().text = Attraction.ToString(".0");
        dateDisplay.GetComponent<TextMeshProUGUI>().text = _day + "/" + _month + "/" + _year;

        _dateTimer += Time.deltaTime;
        if (_dateTimer >= _dayTime)
        {
            _dateTimer = 0f;
            _day += 1;
            if (_day > 30)
            {
                _day = 1;
                _month += 1;
                HandleEconomy();
                if (_month > 12)
                {
                    _month = 1;
                    _year += 1;
                }
            }
        }
    }

    public void HandleEconomy()
    {
        Money -= Maintanence;
        Money += Income;
    }
}

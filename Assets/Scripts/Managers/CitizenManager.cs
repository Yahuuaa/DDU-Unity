using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    public static Dictionary<Guid, Citizen> Citizens = new Dictionary<Guid, Citizen>();
    public static List<Guid> JoblessCitizens = new List<Guid>();
    
    private float _baseInterval = 5.5f;
    private float _minInterval = 0.5f;
    private float _timer = 0f;

    private int _minAmount = 1;
    private int _maxAmount = 5;
    
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= GetCooldown()) //Moving
        {
            if (!ResidentManager.IsResidenceAvailable()) return;
            _timer = 0f;
            int amount = UnityEngine.Random.Range(_minAmount, _maxAmount);
            
            foreach (Guid id in ResidentManager.AvailableResidence.ToList())
            {
                House house = ResidentManager.GetResidence(id);
                if (RoadManager.IsConnectedToHighway(house.GetModel()))
                {
                    house.UpdateRoadIcon(false);
                    if (house.GetPopulationLimit() - house.GetPopulation() >= amount)
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            new Citizen(house.GetID());
                        }
                        return;
                    }

                    amount -= house.GetPopulationLimit() - house.GetPopulation();
                    for (int i = 0; i < house.GetPopulationLimit() - house.GetPopulation(); i++)
                    {
                        new Citizen(house.GetID());
                    }
                }
                else
                {
                    house.UpdateRoadIcon(true);
                }
            }
        }

        if (_timer % 2 == 0) //Work
        {
            // code
        }
    }

    public static Citizen GetCitizen(Guid id)
    {
        return Citizens.GetValueOrDefault(id);
    }
    
    private float GetCooldown()
    {
        return Mathf.Max(_minInterval, _baseInterval - Mathf.Pow(1.5f, CityManager.Attraction));
    }
}
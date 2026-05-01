using System;
using System.Collections.Generic;
using Library;
using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    public static Dictionary<Guid, Citizen> Citizens = new Dictionary<Guid, Citizen>();
    public static float Attraction = 0f;
    
    private float _baseInterval = 5.5f;
    private float _minInterval = 0.5f;
    private float _timer = 0f;

    private int _minAmount = 1;
    private int _maxAmount = 5;

    private float GetCooldown()
    {
        float x = 1.5f;
        return Mathf.Max(_minInterval, _baseInterval - Mathf.Pow(x, Attraction));
    }
    
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= GetCooldown())
        {
            if (!ResidentManager.IsResidenceAvailable()) return;
            _timer = 0f;
            int amount = UnityEngine.Random.Range(_minAmount, _maxAmount);
            
            foreach (Guid id in ResidentManager.AvailableResidence)
            {
                House house = ResidentManager.GetResidence(id);
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
        }
    }
}
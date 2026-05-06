using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    public static Dictionary<Guid, Factory> Workplaces = new Dictionary<Guid, Factory>();
    public static List<Guid> AvailableWorkplaces = new List<Guid>();

    private float _timer = 0f;
    private int _minAmount = 1;
    private int _maxAmount = 5;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 3)
        {
            _timer = 0f;
            int amount = UnityEngine.Random.Range(_minAmount, _maxAmount);
            foreach (Guid workplaceId in AvailableWorkplaces.ToList())
            {
                Factory factory = GetWorkplace(workplaceId);
                if (CitizenManager.JoblessCitizens.Count == 0)
                {
                    factory.UpdateRoadIcon(false);
                    continue;
                }
                foreach (Guid citizenId in CitizenManager.JoblessCitizens.ToList())
                {
                    Citizen citizen = CitizenManager.GetCitizen(citizenId);
                    if (RoadManager.IsBuildingsConnected(ResidentManager.GetResidence(citizen.GetResidence()).GetModel(), factory.GetModel()))
                    {
                        factory.UpdateRoadIcon(false);
                        if (amount > 0 && factory.GetWorkers().Count < factory.GetWorkerLimit())
                        {
                            factory.AddWorker(citizen.GetID());
                            CitizenManager.JoblessCitizens.Remove(citizenId);
                            amount--;
                        }
                        else
                        {
                            return;

                        }
                    }
                    else
                    {
                        factory.UpdateRoadIcon(true);
                    }
                }
            }
        }
    }

    public static Factory GetWorkplace(Guid id)
    {
        return Workplaces.GetValueOrDefault(id);
    }
}
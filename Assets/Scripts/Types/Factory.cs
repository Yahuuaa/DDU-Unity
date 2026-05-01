using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    public class Factory : Building
    {
        private const float IncomeWorkerAddition = 200f;
        private float _income = 0f;

        private int _workerLimit = 5;
        private List<Guid> _workers;
        
        public Factory(GameObject model) : base(model)
        {
            _workers = new List<Guid>();
            
            WorkManager.Workplaces.Add(GetID(), this);
            WorkManager.AvailableWorkplaces.Add(GetID());
        }

        public void Destroy()
        {
            //code
        }

        public void RemoveWorker(Guid citizen)
        {
            _workers.Remove(citizen);
            _income -= IncomeWorkerAddition;
            CitizenManager.GetCitizen(citizen).SetWorking(false);
            if (!WorkManager.AvailableWorkplaces.Contains(GetID())) WorkManager.AvailableWorkplaces.Add(GetID());
        }

        public void AddWorker(Guid citizen)
        {
            _workers.Add(citizen);
            _income += IncomeWorkerAddition;
            CitizenManager.GetCitizen(citizen).SetWorking(true);
            if (_workers.Count >= _workerLimit)
            {
                WorkManager.AvailableWorkplaces.Remove(GetID());
            }
        }

        public float GetIncome()
        {
            return _income;
        }
    }
}
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
        
        private GameObject _spriteRenderer;

        public Factory(GameObject model) : base(model, 100)
        {
            _workers = new List<Guid>();
            
            WorkManager.Workplaces.Add(GetID(), this);
            WorkManager.AvailableWorkplaces.Add(GetID());
            
            _spriteRenderer = model.transform.Find("Sprite Renderer").gameObject;
        }

        public void Destroy()
        {
            foreach (Guid id in _workers)
            {
                RemoveWorker(id);
            }
            UnityEngine.Object.Destroy(GetModel());
            CityManager.Maintanence -= GetMaintenance();
        }
        
        public void UpdateRoadIcon(bool showing)
        {
            _spriteRenderer.SetActive(showing);
        }

        public List<Guid> GetWorkers()
        {
            return _workers;
        }

        public int GetWorkerLimit()
        {
            return _workerLimit;
        }

        public void RemoveWorker(Guid citizen)
        {
            _workers.Remove(citizen);
            _income -= IncomeWorkerAddition;
            CitizenManager.GetCitizen(citizen).SetWorking(false);
            if (!WorkManager.AvailableWorkplaces.Contains(GetID())) WorkManager.AvailableWorkplaces.Add(GetID());
            CityManager.Income -= IncomeWorkerAddition;
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
            CityManager.Income += IncomeWorkerAddition;
        }

        public float GetIncome()
        {
            return _income;
        }
    }
}
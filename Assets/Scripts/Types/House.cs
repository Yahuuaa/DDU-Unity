using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Library
{
    public class House : Building
    {
        private int _populationLimit = 3;
        private List<Guid> _citizens;
        
        public House(GameObject model) : base(model)
        {
            ResidentManager.Residence.Add(GetID(), this);
            ResidentManager.AvailableResidence.Add(GetID());
            _citizens = new List<Guid>();
        }

        public void Destroy()
        {
            foreach (Guid citizen in _citizens)
            {
                CitizenManager.GetCitizen(citizen).Leave();
            }
            ResidentManager.Residence.Remove(GetID());
            Object.Destroy(GetModel());
        }

        public void AddResident(Guid citizen)
        {
            _citizens.Add(citizen);
            if (_citizens.Count == _populationLimit)
            {
                ResidentManager.AvailableResidence.Remove(GetID());
            }
        }

        public void RemoveResident(Guid citizen)
        {
            _citizens.Remove(citizen);
            if (_citizens.Count == _populationLimit)
            {
                if (!ResidentManager.AvailableResidence.Contains(GetID())) ResidentManager.AvailableResidence.Add(GetID());
            }
        }
        
        public int GetPopulationLimit()
        {
            return _populationLimit;
        }

        public int GetPopulation()
        {
            return _citizens.Count;
        }

        public bool HasSpace()
        {
            return _citizens.Count != _populationLimit;
        }
    }
}
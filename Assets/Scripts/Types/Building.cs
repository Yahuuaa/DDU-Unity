using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Library
{
    public class Building
    {
        private Guid _id;
        private GameObject _model;

        private float _maintenance;
        
        public Building(GameObject model, float maintenance)
        {
            _model = model;
            _id = Guid.NewGuid();
            _maintenance = maintenance;
            
            CityManager.Maintanence += maintenance;
            Variables.Object(_model).Set("id", _id.ToString());
        }

        public Guid GetID()
        {
            return _id;
        }

        public GameObject GetModel()
        {
            return _model;
        }
        
        public float GetMaintenance()
        {
            return _maintenance;
        }
    }
}
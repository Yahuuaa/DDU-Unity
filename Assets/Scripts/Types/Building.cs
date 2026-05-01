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
        private float _earnings;
        
        public Building(GameObject model)
        {
            _model = model;
            _id = Guid.NewGuid();
            
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
    }
}
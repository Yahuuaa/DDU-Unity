using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Library
{
    public class Road : Building
    {
        private Guid _roadNetwork;

        public Road(GameObject model) : base(model, 0)
        {
            RoadManager.Roads.Add(GetID(), this);
            
            BoxCollider box = model.GetComponent<BoxCollider>();
            Vector3 worldSize = Vector3.Scale(box.size, model.transform.lossyScale);
            float expand = 0.1f;
            Collider[] hits = Physics.OverlapBox(
                model.transform.position + box.center,
                (worldSize / 2) + new Vector3(expand, expand, expand),
                model.transform.rotation
            );

            foreach (Collider hit in hits)
            {
                if (hit.transform.IsChildOf(model.transform)) continue;
                if (Variables.Object(hit.gameObject).IsDefined("type") && Variables.Object(hit.gameObject).Get<string>("type") == "Road")
                {
                    RoadManager.CheckMergeability(this, Guid.Parse(Variables.Object(hit.gameObject).Get<string>("id")));
                }
            }

            if (HasNetwork()) return;
            
            RoadNetwork network = new RoadNetwork();
            network.AddRoad(GetID());
            SetRoadNetwork(network.GetId());
        }
        
        public void SetShape(RoadShape shape, Quaternion rotation)
        {
            GameObject model = GetModel();
            Vector3 pos = GetModel().transform.position;

            GameObject prefab = shape switch
            {
                RoadShape.Turn      => RoadManager.instance.turnRoad,
                RoadShape.TJunction => RoadManager.instance.tshapeRoad,
                RoadShape.Cross     => RoadManager.instance.crossRoad,
                _                   => RoadManager.instance.straightRoad 
            };

            Object.Destroy(model);
            model = Object.Instantiate(prefab, pos, rotation);
            model.SetActive(true);

            Variables.Object(model).Set("type", "Road");
            Variables.Object(model).Set("id", GetID().ToString());
        }

        public void SetRoadNetwork(Guid roadNetwork)
        {
            _roadNetwork = roadNetwork;
        }

        public bool HasNetwork()
        {
            return _roadNetwork != Guid.Empty;
        }
        
        public void Destroy()
        {
            RoadManager.Roads.Remove(GetID());
            RoadManager.RoadNetworks.GetValueOrDefault(_roadNetwork).RemoveRoad(GetID());
            Object.Destroy(GetModel());
            CityManager.Maintanence -= GetMaintenance();
        }
        
        public Guid GetRoadNetwork()
        {
            return _roadNetwork;
        }
    }
}
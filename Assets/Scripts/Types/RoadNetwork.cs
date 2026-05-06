using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    public class RoadNetwork
    {
        private List<Guid> _roads;
        private List<GameObject> _lines;
        private Guid _id;
        
        public RoadNetwork()
        {
            _id = Guid.NewGuid();
            _roads = new List<Guid>();
            _lines = new List<GameObject>();
            
            RoadManager.RoadNetworks.Add(_id, this);
        }

        public void Merge(RoadNetwork roadNetwork)
        {
            foreach (var road in roadNetwork.GetRoads())
            {
                RoadManager.Roads.GetValueOrDefault(road).SetRoadNetwork(_id);
                AddRoad(road);
            }
            roadNetwork.Destroy();
        }

        public void Destroy()
        {
            RoadManager.RemoveNetwork(this);
        }

        public void AddRoad(Guid road)
        {
            _roads.Add(road);
        }

        public void RemoveRoad(Guid road)
        {
            _roads.Remove(road);
        }

        public List<Guid> GetRoads()
        {
            return _roads;
        }

        public Guid GetId()
        {
            return _id;
        }
    }
}
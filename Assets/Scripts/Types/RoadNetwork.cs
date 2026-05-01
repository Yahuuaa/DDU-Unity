using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    public class RoadNetwork
    {
        private List<Guid> _roads;
        private List<Guid> _houses;
        private List<GameObject> _lines;
        private Guid _id;


        public RoadNetwork()
        {
            _id = Guid.NewGuid();
            _roads = new List<Guid>();
            _houses = new List<Guid>();
            _lines = new List<GameObject>();
            
            RoadManager.RoadNetworks.Add(_id, this);
        }

        public void Merge(RoadNetwork roadNetwork)
        {
            foreach (var road in _roads)
            {
                RoadManager.Roads.GetValueOrDefault(road).SetRoadNetwork(_id);
            }

            //foreach (var road in _roads)
            //{
            //    ResidentManager.Residents.GetValueOrDefault(road).SetRoadNetwork(_id);
            //}
            _roads.AddRange(roadNetwork.GetRoads());
            _houses.AddRange(roadNetwork.GetHouses());
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

        public void AddHouse(Guid house)
        {
            _houses.Add(house);
        }

        public void RemoveHouse(Guid house)
        {
            _houses.Remove(house);
        }

        public List<Guid> GetRoads()
        {
            return _roads;
        }

        public List<Guid> GetHouses()
        {
            return _houses;
        }

        public Guid GetId()
        {
            return _id;
        }


        //Temporary//
        public void HighlightSystem()
        {
            ClearHighlight();
            
            List<Road> roads = new List<Road>();
            foreach (var roadGuid in _roads)
            {
                Road road = RoadManager.Roads.GetValueOrDefault(roadGuid);
                if (road != null) roads.Add(road);
            }

            for (int i = 0; i < roads.Count; i++)
            {
                float closestDist = float.MaxValue;
                Road closestRoad = null;
                Vector3 posA = roads[i].GetModel().transform.position + Vector3.up * 1f;

                for (int j = 0; j < roads.Count; j++)
                {
                    if (i == j) continue;
                    Vector3 posB = roads[j].GetModel().transform.position + Vector3.up * 1f;
                    float dist = Vector3.Distance(posA, posB);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestRoad = roads[j];
                    }
                }

                if (closestRoad != null)
                {
                    CreateLine(posA, closestRoad.GetModel().transform.position + Vector3.up * 1f);
                }
            }
        }

        private void CreateLine(Vector3 start, Vector3 end)
        {
            GameObject lineObj = new GameObject("NetworkLine_" + _lines.Count);
            
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.green;
            lr.endColor = Color.green;

            _lines.Add(lineObj);
        }

        public void ClearHighlight()
        {
            foreach (GameObject line in _lines)
            {
                UnityEngine.Object.Destroy(line);
            }

            _lines.Clear();
        }
    }
}
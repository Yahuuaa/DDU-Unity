using System;
using System.Collections.Generic;
using Library;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static Dictionary<Guid, Road> Roads = new Dictionary<Guid, Road>();
    public static Dictionary<Guid, RoadNetwork> RoadNetworks = new Dictionary<Guid, RoadNetwork>();

    void Update() //Temporary
    {
        foreach (RoadNetwork network in RoadNetworks.Values)
        {
            network.HighlightSystem();
        }
    }

    public static Road GetRoad(Guid id)
    {
        return Roads.GetValueOrDefault(id);
    }

    public static RoadNetwork GetRoadNetwork(Guid id)
    {
        return RoadNetworks.GetValueOrDefault(id);
    }
    
    public static void AddNetwork(RoadNetwork roadNetwork)
    {
        RoadNetworks.Add(roadNetwork.GetId(), roadNetwork);
    }

    public static void RemoveNetwork(RoadNetwork roadNetwork)
    {
        RoadNetworks.Remove(roadNetwork.GetId());
    }
     
    public static void CheckMergeability(Road road1, Guid road2)
    {
        Road road = GetRoad(road2);
        if (road == null)
        {
            Debug.LogWarning("Road not found in RoadManager: " + road2);
            return;
        }

        if (SharesFullEdge(road1.GetModel(), road.GetModel()))
        {
            if (road.HasNetwork() && road1.HasNetwork() && road.GetRoadNetwork() != road1.GetRoadNetwork())
            {
                GetRoadNetwork(road.GetRoadNetwork()).Merge(GetRoadNetwork(road1.GetRoadNetwork()));
            }
            else if (road.HasNetwork() && !road1.HasNetwork())
            {
                RoadNetwork roadNetwork = GetRoadNetwork(road.GetRoadNetwork());
                roadNetwork.AddRoad(road1.GetID());
                road1.SetRoadNetwork(roadNetwork.GetId());
            }
        }
    }
    
    public static bool SharesFullEdge(GameObject model1, GameObject model2)
    {
        Vector3 posA = model1.transform.position;
        Vector3 posB = model2.transform.position;
        Vector3 diff = posB - posA;

        BoxCollider boxA = model1.GetComponent<BoxCollider>();
        Vector3 sizeA = Vector3.Scale(boxA.size, model1.transform.lossyScale);

        Debug.Log($"diff: {diff}, sizeA: {sizeA}");

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
        {
            float overlapZ = sizeA.z - Mathf.Abs(diff.z);
            Debug.Log($"X adjacent, overlapZ: {overlapZ}, required: {sizeA.z}");
            return overlapZ >= sizeA.z;
        }
        else
        {
            float overlapX = sizeA.x - Mathf.Abs(diff.x);
            Debug.Log($"Z adjacent, overlapX: {overlapX}, required: {sizeA.x}");
            return overlapX >= sizeA.x;
        }
    }
}

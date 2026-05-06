using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using Unity.VisualScripting;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static Dictionary<Guid, Road> Roads = new Dictionary<Guid, Road>();
    public static Dictionary<Guid, RoadNetwork> RoadNetworks = new Dictionary<Guid, RoadNetwork>();

    public GameObject turnRoad;
    public GameObject tshapeRoad;
    public GameObject crossRoad;
    public GameObject straightRoad;

    public GameObject highwayRoad;
    public static Guid highwayRoadId;

    public static RoadManager instance;
    
    void Start()
    {
        instance = this;
        
        Road road = new Road(highwayRoad);
        highwayRoadId = road.GetID();
    }
    
    void Update() //Temporary
    {
        foreach (RoadNetwork network in RoadNetworks.Values)
        {
            network.HighlightSystem();
        }
    }

    public static bool IsConnectedToHighway(GameObject model)
    {
        BoxCollider box = model.GetComponent<BoxCollider>();
        Vector3 worldSize = Vector3.Scale(box.size, model.transform.lossyScale);
        float expand = 0.1f;
        Collider[] hits = Physics.OverlapBox(
            model.transform.position + box.center,
            (worldSize / 2) + new Vector3(expand, expand, expand),
            model.transform.rotation
        );
        
        List<Guid> roadNetworks = new List<Guid>();
        foreach (Collider hit in hits)
        {
            if (hit.transform.IsChildOf(model.transform)) continue;
            if (hit.transform.name == "showcase") continue;
            if (Variables.Object(hit.gameObject).IsDefined("type") && Variables.Object(hit.gameObject).Get<string>("type") == "Road")
            {
                Road road = GetRoad(Guid.Parse(Variables.Object(hit.gameObject).Get<string>("id")));
                if (SharesFullEdge(hit.gameObject, model)) roadNetworks.Add(road.GetRoadNetwork());
            }

        }
        return roadNetworks.Contains(GetRoad(highwayRoadId).GetRoadNetwork());
    }

    public static bool IsBuildingsConnected(GameObject building1, GameObject building2)
    {
        List<GameObject> buildings = new List<GameObject>();
        List<Guid> roadNetworks = new List<Guid>();
        buildings.Add(building1);
        buildings.Add(building2);
        foreach (GameObject road in buildings)
        {
            BoxCollider box = road.GetComponent<BoxCollider>();
            Vector3 worldSize = Vector3.Scale(box.size, road.transform.lossyScale);
            float expand = 0.1f;
            Collider[] hits = Physics.OverlapBox(
                road.transform.position + box.center,
                (worldSize / 2) + new Vector3(expand, expand, expand),
                road.transform.rotation
            );

            List<Guid> connections = new List<Guid>();
            foreach (Collider hit in hits)
            {
                if (hit.transform.IsChildOf(road.transform)) continue;
                if (hit.transform.name == "showcase") continue;
                if (Variables.Object(hit.gameObject).IsDefined("type") && Variables.Object(hit.gameObject).Get<string>("type") == "Road")
                {
                    Guid network = GetRoad(Guid.Parse(Variables.Object(hit.gameObject).Get<string>("id"))).GetRoadNetwork();
                    if (SharesFullEdge(hit.gameObject, road) && !connections.Contains(network)) connections.Add(network);
                }
            }
            roadNetworks.AddRange(connections);
        }
        return roadNetworks.Count != roadNetworks.Distinct().Count();
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
        BoxCollider boxA = model1.GetComponent<BoxCollider>();
        BoxCollider boxB = model2.GetComponent<BoxCollider>();

        Vector3 centerA = model1.transform.TransformPoint(boxA.center);
        Vector3 centerB = model2.transform.TransformPoint(boxB.center);
        Vector3 diff = centerB - centerA;

        Vector3 sizeA = GetWorldAABBSize(boxA);
        Vector3 sizeB = GetWorldAABBSize(boxB);

        float overlapThresholdRatio = 0.9f;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
        {
            float overlapZ = (sizeA.z / 2f) + (sizeB.z / 2f) - Mathf.Abs(diff.z);
            float minEdge = Mathf.Min(sizeA.z, sizeB.z);
            return overlapZ >= minEdge * overlapThresholdRatio;
        }
        else
        {
            float overlapX = (sizeA.x / 2f) + (sizeB.x / 2f) - Mathf.Abs(diff.x);
            float minEdge = Mathf.Min(sizeA.x, sizeB.x);
            return overlapX >= minEdge * overlapThresholdRatio;
        }
    }

    private static Vector3 GetWorldAABBSize(BoxCollider box)
    {
        Vector3 c = box.center;
        Vector3 e = box.size * 0.5f;
        Transform t = box.transform;

        Vector3[] corners = new Vector3[8];
        int i = 0;
        foreach (float sx in new[] { -1f, 1f })
        foreach (float sy in new[] { -1f, 1f })
        foreach (float sz in new[] { -1f, 1f })
            corners[i++] = t.TransformPoint(c + new Vector3(e.x * sx, e.y * sy, e.z * sz));

        Vector3 min = corners[0], max = corners[0];
        for (int j = 1; j < 8; j++)
        {
            min = Vector3.Min(min, corners[j]);
            max = Vector3.Max(max, corners[j]);
        }
        return max - min;
    }
}

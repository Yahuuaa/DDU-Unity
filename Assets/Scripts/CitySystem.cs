using System.Numerics;
using UnityEngine;
using System.Collections.Generic;

public class CitySystem : MonoBehaviour
{
    void Start()
    {  
    }

    void Update()
    {
    }

    private static int currentID = 0;

    public static int generateID()
    {
        currentID += 1;
        return currentID;
    }

    public static Dictionary<int, Building> buildings = new Dictionary<int, Building>();
    public static Dictionary<UnityEngine.Vector3, int> postions = new Dictionary<UnityEngine.Vector3, int>();
    
    public static int addBuilding(Building building)
    {
        int ID = generateID();
        buildings.Add(ID, building);
        postions.Add(building.GetModel().transform.position, ID);
        return ID;
    }

    public static void removeBuilding(int ID)
    {
        postions.Remove(buildings.GetValueOrDefault(ID).GetModel().transform.position);
        buildings.Remove(ID);
    }
}

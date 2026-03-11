using UnityEngine;

public class Building
{
    public int ID;
    public string Type;
    public GameObject Structure;

    public float Income;
    public float Maintenance;
    public int Population;
    public int PopulationLimit;
    public float Happiness;
    public float Health;

    public Building(GameObject model, float maintenance, int populationLimit, string type)
    {
        Structure = model;
        Maintenance = maintenance;
        PopulationLimit = populationLimit;
        Type = type;

        Income = 0f;
        Population = 0;
        Happiness = 50f;
        Health = 50f;

        ID = CitySystem.addBuilding(this);
    }

    public void Delete()
    {
        UnityEngine.Object.Destroy(Structure);
        CitySystem.removeBuilding(ID);
    }

    public GameObject GetModel()
    {
        return this.Structure;
    }
}
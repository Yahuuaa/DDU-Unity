using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Diagnostics;
using System.ComponentModel;

public class GridSystem : MonoBehaviour
{

    public GameObject buildingShowcase;
    public bool isBuilding = false;
    public string type;

    public int gridMin = -10;
    public int gridMax = 10;

    void Update()
    {
        if (isBuilding)
        {
            DrawGrid();
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Destroy(buildingShowcase);
                isBuilding = false;
                HideGrid();
                return;
            }
            Vector3 mousePos = GetMouse3DPos();
            int gridX = Mathf.FloorToInt(mousePos.x * 2f) / 2;
            int gridZ = Mathf.FloorToInt(mousePos.z * 2f) / 2;

            if (canBePlaced(gridX, gridZ))
            {
                buildingShowcase.transform.position = new Vector3(gridX, 0, gridZ);
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    createBuilding();
                }
            } else
            {
                buildingShowcase.transform.position = new Vector3(gridX, 0, gridZ);
            }
        }
    }

    public bool canBePlaced(int x, int y)
    {
        return true;
    }

    public void spawnBuilding(string buildingName)
    {
        GameObject buildings = GameObject.Find("Buildings");
        GameObject building = buildings.transform.Find(buildingName).gameObject;

        if (!isBuilding)
        {
            isBuilding = true;
            type = buildingName;
            buildingShowcase = Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity);
            buildingShowcase.SetActive(true);
        }
    }

    void createBuilding()
    {
        GameObject build = Instantiate(buildingShowcase, buildingShowcase.transform.position, Quaternion.identity);
        Destroy(buildingShowcase);
        isBuilding = false;
        HideGrid();

        Building model = new Building(build, 0f, 3, ty);
    }


    private List<GameObject> gridLines = new List<GameObject>();

    public void DrawGrid()
    {
        if (gridLines.Count > 0)
            return;

        for (int x = gridMin; x <= gridMax; x++)
        {
            CreateLine(
                new Vector3(x, 0, gridMin),
                new Vector3(x, 0, gridMax)
            );
        }

        for (int z = gridMin; z <= gridMax; z++)
        {
            CreateLine(
                new Vector3(gridMin, 0, z),
                new Vector3(gridMax, 0, z)
            );
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = transform; 

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default")); 
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;

        gridLines.Add(lineObj);
    }

    public void HideGrid()
    {
        foreach (GameObject line in gridLines)
        {
            Destroy(line); 
        }
        gridLines.Clear();
    }

    Vector3 GetMouse3DPos() { 
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); 
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); 
        if (groundPlane.Raycast(ray, out float distance)) 
        { 
            return ray.GetPoint(distance); 
        }
        return Vector3.zero;
    }
}

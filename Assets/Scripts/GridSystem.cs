using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class GridSystem : MonoBehaviour
{

    public GameObject buildingShowcase;

    public Dictionary<string, Dictionary<string, object>> buildingTemp =
    new Dictionary<string, Dictionary<string, object>>
    {   
        {
            "House-1", new Dictionary<string, object>
            {
                {"sizeX", 1},
                {"sizeZ", 1},
            }
        },
        {
            "Factory-1", new Dictionary<string, object>
            {
                {"sizeX", 2},
                {"sizeZ", 2},
            }
        }
    };
    public Dictionary<string, GridCell> grids = new Dictionary<string, GridCell>();
    public bool isBuilding = false;

    public int gridMin = -10;
    public int gridMax = 10;
    public float cellSize = 2f;

    void Start()
    {
        for (int x = gridMin; x < gridMax; x++)
        {
            for (int z = gridMin; z < gridMax; z++)
            {
                string gridId = $"{x}_{z}";
                grids[gridId] = new GridCell(x, z);
            }
        }
    }

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
            int gridX = Mathf.FloorToInt(mousePos.x / cellSize);
            int gridZ = Mathf.FloorToInt(mousePos.z / cellSize);

            if (cellExists(gridX, gridZ))
            {
                if (!isOccupied(gridX, gridZ))
                {
                    buildingShowcase.transform.position = new Vector3(gridX * cellSize, 0, gridZ * cellSize);
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        createBuilding();
                    }
                } else
                {
                    buildingShowcase.transform.position = new Vector3(gridX * cellSize, 0, gridZ * cellSize);
                }
            }
        }
    }

    public void spawnBuilding(string buildingName)
    {
        GameObject buildings = GameObject.Find("Buildings");
        GameObject building = buildings.transform.Find(buildingName).gameObject;

        if (!isBuilding)
        {
            isBuilding = true;
            buildingShowcase = Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity);
            buildingShowcase.SetActive(true);
        }
    }

    void createBuilding()
    {
        Destroy(buildingShowcase);
        isBuilding = false;
        HideGrid();
        //Build
    }

    bool cellExists(int x, int z)
    {
        string gridId = $"{x}_{z}";
        return grids.ContainsKey(gridId);
    }

    bool isOccupied(int x, int z)
    {
        string gridId = $"{x}_{z}";
        return grids[gridId].isOccupied;
    }

    private List<GameObject> gridLines = new List<GameObject>();

    public void DrawGrid()
    {
        if (gridLines.Count > 0)
            return;

        for (int x = gridMin; x <= gridMax; x++)
        {
            CreateLine(
                new Vector3(x * cellSize, 0.01f, gridMin * cellSize),
                new Vector3(x * cellSize, 0.01f, gridMax * cellSize)
            );
        }

        for (int z = gridMin; z <= gridMax; z++)
        {
            CreateLine(
                new Vector3(gridMin * cellSize, 0.01f, z * cellSize),
                new Vector3(gridMax * cellSize, 0.01f, z * cellSize)
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
        lr.startColor = Color.green;
        lr.endColor = Color.green;

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

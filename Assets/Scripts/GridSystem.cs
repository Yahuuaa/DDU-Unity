using UnityEngine;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour
{

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

    void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
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

        }
    }

    void spawnBuilding()
    {
        
    }

    void createBuilding()
    {
        
    }

    Vector3 getMouse3DPos()
    {
        var dist = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        var v3Pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);   
        return v3Pos;
    }
}

using Library;
using Library.Library;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    
    //Settings
    public float cameraSpeed = 10f;
    public float cameraZoom = 5f;
    
    public float soundVolume = 100f;
    public float musicVolume = 100f;
    
    //Game Properties
    public string gameName = "";
    
    public double startMoney = 50000;
    public Difficulty difficulty = Difficulty.Nemt;

    public int terrainSize = 20;
    public TerrainTheme terrainTheme = TerrainTheme.grasslands;
    
    void Start()
    {
        instance = new MenuManager();
    }

    void Update()
    {
        
    }
    
    //Update Methods
    public static void UpdateMoney(double amount, GameObject container)
    {
        if (instance.startMoney + amount < 4000 && instance.startMoney + amount < 500000) return;
        instance.startMoney += amount;
        container.GetComponent<TextMeshPro>().text = instance.startMoney.ToString("N0");
    }
    
}

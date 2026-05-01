using System.Globalization;
using Library;
using Library.Library;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
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
    public TerrainTheme terrainTheme = TerrainTheme.Grasslands;
    
    //Display Containers
    public GameObject gameNameContainer;
    
    public GameObject cameraSpeedContainer;
    public GameObject cameraZoomContainer;
    
    public GameObject soundVolumeContainer;
    public GameObject musicVolumeContainer;
    
    public GameObject startMoneyContainer;
    public GameObject difficultyContainer;

    public GameObject terrainSizeContainer;
    public GameObject terrainThemeContainer;

    public void LoadGameScene()
    {
        if (_isValid)
        {
            SceneManager.LoadScene("Game");
        }
    }
    
    public void LoadGame() //Disabled Feature
    {
        NotificationManager.createMessage("Funktion Utilgængelig", "Denne funktion er ikke tilgængelig!", new Color(255, 234, 0));
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    //Display Methods - Settings
    private float _cameraSpeedMin = 1f;
    private float _cameraSpeedMax = 25f;
    public void UpdateCameraSpeed(float num)
    {
        cameraSpeed += num;
        if (cameraSpeed > _cameraSpeedMax) cameraSpeed = _cameraSpeedMax;
        if (cameraSpeed < _cameraSpeedMin) cameraSpeed = _cameraSpeedMin;
        cameraSpeedContainer.GetComponent<TextMeshProUGUI>().text = cameraSpeed.ToString("N1", CultureInfo.InvariantCulture);
    }

    private float _cameraZoomMin = 1f;
    private float _cameraZoomMax = 25f;
    public void UpdateCameraZoom(float num)
    {
        cameraZoom += num;
        if  (cameraZoom > _cameraZoomMax) cameraZoom = _cameraZoomMax;
        if (cameraZoom < _cameraZoomMin) cameraZoom = _cameraZoomMin;
        cameraZoomContainer.GetComponent<TextMeshProUGUI>().text = cameraZoom.ToString("N1", CultureInfo.InvariantCulture);
    }

    private float _soundVolumeMin = 0f;
    private float _soundVolumeMax = 100f;
    public void UpdateSoundVolume(float num)
    {
        soundVolume += num;
        if (soundVolume > _soundVolumeMax) soundVolume = _soundVolumeMax;
        if (soundVolume < _soundVolumeMin) soundVolume = _soundVolumeMin;
        soundVolumeContainer.GetComponent<TextMeshProUGUI>().text = soundVolume.ToString("N1", CultureInfo.InvariantCulture);
    }

    private float _musicVolumeMin = 0f;
    private float _musicVolumeMax = 100f;
    public void UpdateMusicVolume(float num)
    {
        musicVolume += num;
        if (musicVolume > _musicVolumeMax) musicVolume = _musicVolumeMax;
        if (musicVolume < _musicVolumeMin) musicVolume = _musicVolumeMin;
        musicVolumeContainer.GetComponent<TextMeshProUGUI>().text = musicVolume.ToString("N1", CultureInfo.InvariantCulture);
    }

    //Display Methods - Game Creation
    private bool _isValid = false;
    private int _gameNameMax = 15;
    private int _gameNameMin = 5;
    public void UpdateGameName()
    {
        gameName = gameNameContainer.GetComponent<TextMeshProUGUI>().text;
        if (gameName.Length >= _gameNameMin && gameName.Length <= _gameNameMax)
        {
            _isValid = true;
        }
        else
        {
            _isValid = false;
        }
    }
    
    private int _startMoneyMin = 10000;
    private int _startMoneyMax = 500000;
    public void UpdateMoney(float amount)
    {
        if (startMoney + amount < _startMoneyMin || startMoney + amount > _startMoneyMax) return;
        startMoney += amount;
        startMoneyContainer.GetComponent<TextMeshProUGUI>().text = startMoney.ToString("N0");
    }

    public void UpdateDifficulty(bool previous)
    {
        if (previous)
        {
            difficulty = difficulty.PreviousDifficulty();
        }
        else
        {
            difficulty = difficulty.NextDifficulty();
        }
        difficultyContainer.GetComponent<TextMeshProUGUI>().text = difficulty.GetName();
    }

    private int _terrainSizeMin = 15;
    private int _terrainSizeMax = 50;
    public void UpdateTerrainSize(bool previous)
    {
        if (previous)
        {
            terrainSize -= 2;
        }
        else
        {
            terrainSize += 2;
        }
        

        if (terrainSize > _terrainSizeMax) terrainSize = _terrainSizeMax;
        if (terrainSize < _terrainSizeMin) terrainSize = _terrainSizeMin;
        terrainSizeContainer.GetComponent<TextMeshProUGUI>().text = terrainSize + "x" + terrainSize;
    }

    public void UpdateTerrainTheme(bool previous)
    {
        if (previous)
        {
            terrainTheme = terrainTheme.PreviousTheme();
        }
        else
        {
            terrainTheme = terrainTheme.NextTheme();
        }
        terrainThemeContainer.GetComponent<TextMeshProUGUI>().text = terrainTheme.GetName();
    }
}

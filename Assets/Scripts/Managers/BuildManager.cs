using Library;
using UnityEngine;
using Grid = Library.Grid;

public class BuildManager : MonoBehaviour
{
    public GameObject buildIcon;
    
    private Showcase _showcase;
    private Grid _grid;

    private Camera _camera;
    private readonly Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);

    private float _notificationCooldown = 0.5f;
    private float _lastNotificationTime = -Mathf.Infinity;
    
    void Start()
    {
        _camera = Camera.main;
        _grid = new Grid(-50, -50, 50, 50, transform);
    }

    void Update()
    {
        if (_showcase != null && _showcase.IsVisible())
        {
            if (Input.GetMouseButton(1)) //Cancel
            {
                _grid.HideGrid();
                _showcase.SetVisible(false);
                buildIcon.SetActive(true);
                return;
            }
            if (Input.GetKeyDown(KeyCode.R)) //Rotation
            {
                _showcase.Rotate();
            }
            if (Input.GetMouseButton(0)) //Placing
            {
                if (_showcase.CanBePlaced())
                {
                    _showcase.PlaceShowcase();
                    _grid.HideGrid();
                    buildIcon.SetActive(true);
                }
                else
                {
                    if (Time.time >= _lastNotificationTime + _notificationCooldown)
                    {
                        NotificationManager.createMessage("Utilgængelig Plads", "Der er ikke plads til bygningen ved den position", new Color(255, 23, 0));
                        _lastNotificationTime = Time.time;
                    }
                }
            }   
            Vector3 position = GetMousePosition();
            _showcase.Move((int) position.x, (int) position.z);
        }
    }
    
    public void StartShowcase(GameObject building)
    {
        _grid.ShowGrid();
        if (_showcase != null)
        {
            _showcase.ChangeModel(building);
        }
        else
        {
            _showcase = new Showcase(building);
        }

        _showcase.SetVisible(true);
        
    }
    
    public Vector3 GetMousePosition()
    {
        Ray ray = _camera.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
        return _groundPlane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : Vector3.zero;
    }
    
    //Notifications
    public void SendNotification()
    {
        NotificationManager.createMessage("Funktion Utilgængelig", "Denne funktion er ikke tilgængelig!", new Color(225, 234, 0));  
    }
}

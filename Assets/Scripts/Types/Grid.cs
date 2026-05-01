using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    public class Grid
    {
        private int _startX, _startY, _endX, _endY;
        private List<GameObject> _lines;
        private Transform _parent;
        private bool _active = false;

        public Grid(int startX, int startY, int endX, int endY, Transform parent)
        {
            _startX = startX;
            _startY = startY;
            _endX = endX;
            _endY = endY;
            _parent = parent;
            _lines = new List<GameObject>();
        }

        public void ShowGrid()
        {
            if (_lines.Count > 0)
            {
                Debug.Log("Grid has already been initiated.");
                return;
            }

            _active = true;

            for (int x = _startX; x <= _endX; x++)
            {
                CreateLine(new Vector3(x, 0, _startY), new Vector3(x, 0, _endY));
            }

            for (int z = _startY; z <= _endY; z++)
            {
                CreateLine(new Vector3(_startX, 0, z), new Vector3(_endX, 0, z));
            }
        }

        public void HideGrid()
        {
            if (_lines.Count > 0)
            {
                _active = false;
                foreach (GameObject line in _lines)
                {
                    UnityEngine.Object.Destroy(line); 
                }
                _lines.Clear();
            }
            else
            {
                Debug.Log("Grid has not been initiated.");
            }
        }

        public bool isActive()
        {
            return _active;
        }

        private void CreateLine(Vector3 start, Vector3 end)
        {
            GameObject lineObj = new GameObject("GridLine");
            lineObj.transform.parent = _parent;

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.darkGreen;
            lr.endColor = Color.darkGreen;

            _lines.Add(lineObj);
        }
    }
}
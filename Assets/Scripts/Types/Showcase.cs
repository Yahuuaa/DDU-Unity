using Unity.VisualScripting;
using UnityEngine;

namespace Library
{
    public class Showcase
    {
        private GameObject _model;
        private Vector3 _position;
        private BuildType _type;

        private float _offsetX = 0f;
        private float _offsetZ = 0f;

        private float _originX;
        private float _originZ;

        private float _price = 0f;
        
        public Showcase(GameObject model)
        {
            _model = Object.Instantiate(model, model.transform.position, model.transform.rotation);
            _model.transform.name = "showcase";
            _position = _model.transform.position;
            _originX = _position.x;
            _originZ = _position.z;
            
            if (Variables.Object(model).IsDefined("price"))
            {
                _price = Variables.Object(model).Get<float>("price");
            }
            else
            {
                _price = 100000f;
            }
            
            RecalculateOffset();
            _model.SetActive(false);
        }

        private void RecalculateOffset()
        {
            BoxCollider box = _model.GetComponent<BoxCollider>();
            Vector3 worldSize = Vector3.Scale(box.size, _model.transform.lossyScale);

            int sizeX = Mathf.RoundToInt(worldSize.x);
            int sizeZ = Mathf.RoundToInt(worldSize.z);
            
            _offsetX = (sizeX % 2 == 1) ? 0.5f : 0f;
            _offsetZ = (sizeZ % 2 == 1) ? 0.5f : 0f;
        }

        public void SetVisible(bool visible)
        {
            if (visible) _model.SetActive(true);
            else _model.SetActive(false);
        }

        public bool IsVisible()
        {
            return _model.activeSelf;
        }

        public void Move(int x, int z)
        {
            _position.x = x + _offsetX + _originX;
            _position.z = z + _offsetZ + _originZ;
            _model.transform.position = _position;
        }
        
        public void Rotate()
        {
            _model.transform.Rotate(0,0,90);
            (_offsetX, _offsetZ) = (_offsetZ, _offsetX);
            float tempX = _originX;
            _originX = _originZ;
            _originZ = -tempX;
        }

        public void ChangeModel(GameObject model)
        {
            Object.Destroy(_model);
            _model = Object.Instantiate(model, model.transform.position, model.transform.rotation);
            _model.transform.name = "showcase";
            _position = _model.transform.position;
            _originX = _position.x;
            _originZ = _position.z;
            RecalculateOffset();

            if (Variables.Object(model).IsDefined("price"))
            {
                _price = Variables.Object(model).Get<float>("price");
            }
            else
            {
                _price = 100000f;
            }
        }

        public bool CanBePlaced()
        {
            BoxCollider box = _model.GetComponent<BoxCollider>();
            float shrink = 0.05f;

            Vector3 worldCenter = _model.transform.TransformPoint(box.center);
            Vector3 halfExtents = Vector3.Scale(box.size / 2f, _model.transform.lossyScale) - new Vector3(shrink, shrink, shrink);
            Collider[] hits = Physics.OverlapBox(
                worldCenter,
                halfExtents,
                _model.transform.rotation
            );

            foreach (Collider hit in hits)
            {
                if (hit.transform.IsChildOf(_model.transform)) continue;
                return false;
            }
            return true;
        }

        public bool CanAfford()
        {
            return CityManager.Money >= _price;
        }

        public void PlaceShowcase()
        {
            CityManager.Money -= _price;
            _model.SetActive(false);
            GameObject build = Object.Instantiate(_model, _model.transform.position, _model.transform.rotation);
            build.SetActive(true);
            string type = Variables.Object(_model).Get<string>("type");
            
            if (type == "House")
            {
                new House(build);
            }
            else if (type == "Road")
            {
                new Road(build);
            }
            else if (type == "Factory")
            {
                new Factory(build);
            }
            else if (type == "Hospital")
            {
                //Building building = new Hospital(build);
            }
            _model.SetActive(false);
        }
    }
}
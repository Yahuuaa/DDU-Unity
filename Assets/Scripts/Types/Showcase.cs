using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Library
{
    public class Showcase
    {
        private GameObject _model;
        private Vector3 _position;
        private BuildType _type;

        private float _originX;
        private float _originZ;

        private float _tempX;

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
            
            _model.SetActive(false);
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
            _position.x = x + _originX + _tempX;
            _position.z = z + _originZ;
            _model.transform.position = _position;
        }
        
        public void Rotate()
        {
            _model.transform.Rotate(0, 90,0, Space.World);
            float tempX = _originX;
            _originX = -_originZ;
            _originZ = tempX;
            
            if (Variables.Object(_model).IsDefined("rotationOffset") &&
                ((int)_model.transform.eulerAngles.y == 0 || (int)_model.transform.eulerAngles.y == 180))
            {
                _tempX = 0.38f;
            }
            else
            {
                _tempX = 0f;
            }
        }

        public void ChangeModel(GameObject model)
        {
            Object.Destroy(_model);
            _model = Object.Instantiate(model, model.transform.position, model.transform.rotation);
            _model.transform.name = "showcase";
            _position = _model.transform.position;
            _originX = model.transform.position.x;
            _originZ = model.transform.position.z;
            _tempX = 0;

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
            float shrink = 0.55f;

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
                if (Variables.Object(hit.gameObject).IsDefined("id")) return false;
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
            GameObject build = Object.Instantiate(_model, _model.transform.position, _model.transform.rotation);
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
                //new Hospital(build);
            }
        }
    }
}
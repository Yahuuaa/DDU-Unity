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
        
        public Showcase(GameObject model)
        {
            _model = Object.Instantiate(model, model.transform.position, model.transform.rotation);
            _position = _model.transform.position;
            
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
            _position.x = x + _offsetX;
            _position.z = z + _offsetZ;
            _model.transform.position = _position;
        }
        
        public void Rotate()
        {
            _model.transform.Rotate(0,90,0);
            (_offsetX, _offsetZ) = (_offsetZ, _offsetX);
        }

        public void ChangeModel(GameObject model)
        {
            _model = Object.Instantiate(model, model.transform.position, model.transform.rotation);
            _position = _model.transform.position;
            RecalculateOffset();
        }

        public bool CanBePlaced()
        {
            BoxCollider box = _model.GetComponent<BoxCollider>();
            float shrink = 0.1f;
            Collider[] hits = Physics.OverlapBox(
                _model.transform.position + box.center,
                (box.size / 2) - new Vector3(shrink, shrink, shrink),
                _model.transform.rotation
            );

            foreach (Collider hit in hits)
            {
                if (hit.transform.IsChildOf(_model.transform)) continue;
                return false;
            }
            return true;
        }

        public void PlaceShowcase()
        {
            _model.SetActive(false);
            GameObject build = Object.Instantiate(_model, _model.transform.position, _model.transform.rotation);
            build.SetActive(true);
            string type = Variables.Object(_model).Get<string>("type");
            
            
            if (type == "House")
            {
                Building building = new House(build);
            }
            else if (type == "Road")
            {
                Building building = new Road(build);
            }
            else if (type == "Factory")
            {
                //Building building = new Factory(build);
            }
            else if (type == "Hospital")
            {
                //Building building = new Hospital(build);
            }
            _model.SetActive(false);
        }
    }
}
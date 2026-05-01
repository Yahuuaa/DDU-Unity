using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Library
{
    public class Notification
    {
        private GameObject _model;

        private float _timer;

        
        public Notification(string title, string errorMessage, GameObject model, Color color)
        {
            _model = Object.Instantiate(model, model.transform.position, model.transform.rotation, model.transform.parent);            _model.SetActive(true);
            _model.GetComponent<Image>().color = color;
            _timer = 5f * 60f;
            
            _model.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = errorMessage;
            _model.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = title;
        }

        public void MoveModel()
        {
            _model.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 200f);
        }

        public void DecreaseTimer(float decrease)
        {
            _timer -= decrease;
            if (_timer <= 0f)
            {
                Object.Destroy(_model);
                NotificationManager.messages.Remove(this);
            }
            _model.GetComponent<CanvasGroup>().alpha = (1f / 300f) *  _timer;
        }
    }
}
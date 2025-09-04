using Service_Locator;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] private string _saveString;
        [SerializeField] private Color _offStateColor = Color.white;
        [SerializeField] private Color _onStateColor = Color.white;

        [Header("References")] 
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _rectTransform;

        private bool _state = true;
        public bool State => _state;

        public Action OnToggle;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(Toggle);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Toggle);
        }

        public void SetState(bool on)
        {
            var targetPos = _state
                ? _button.GetComponent<RectTransform>().rect.width / 2
                : _rectTransform.rect.width - (_button.GetComponent<RectTransform>().rect.width / 2);
            _button.GetComponent<Image>().color = _state ? _onStateColor : _offStateColor;
            _button.GetComponent<RectTransform>().anchoredPosition = new Vector2(targetPos, _rectTransform.localPosition.y);
            _state = on;
            OnToggle?.Invoke();
        }

        private void Toggle()
        {
            SetState(!_state);
        }
    }
}
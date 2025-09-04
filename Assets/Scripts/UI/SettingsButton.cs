using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsButton : MonoBehaviour
    {
        private Button _button;
        private UIManager _uiManager;

        private void Awake()
        {
            _uiManager = UIManager.Instance;
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _uiManager.ShowDialog("Settings");
        }
    }
}
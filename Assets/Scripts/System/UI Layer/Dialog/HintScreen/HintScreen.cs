using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintScreen : AUIScreenController<HintProperties>
{
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private Button _closeButton;

    void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        UIManager.Instance.HideDialog(ScreenID);
    }

    protected override void OnPropertiesSet()
    {
        _hintText.text = Properties.HintText;
    }
}
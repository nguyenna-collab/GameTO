using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBonous : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0, 120)] private int _bonousTime;

    [Header("References")]
    [SerializeField] private GameplayTimer _gameplayTimer;
    [SerializeField] private TMP_Text _bonousText;
    [SerializeField] private Button _bonousButton;
    [SerializeField] private GameObject _adsImage;

    void Start()
    {
        _bonousText.text = $"+{_bonousTime}s";
    }

    void OnEnable()
    {
        _bonousButton.onClick.AddListener(UseBonous);
    }

    void OnDisable()
    {
        _bonousButton.onClick.RemoveListener(UseBonous);
    }

    private void UseBonous()
    {
        _gameplayTimer.AddTime(_bonousTime);
        _bonousText.gameObject.SetActive(false);
        _adsImage.SetActive(true);
    }
}
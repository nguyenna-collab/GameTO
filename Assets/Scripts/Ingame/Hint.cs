using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HintDataSO _hintData;
    [SerializeField] private TMP_Text _hintAmountText;
    [SerializeField] private Button _useHintButton;
    [SerializeField] private GameObject _adsImage;

    [Header("Settings")]
    [SerializeField] private int _hintAmount = 1;

    public int GetHintAmount { get { return _hintAmount; } }

    private int _currentHintIndex = 0;

    void Awake()
    {
        _hintAmountText ??= GetComponentInChildren<TMP_Text>();
        _useHintButton ??= GetComponentInChildren<Button>();
        UpdateHintAmountText();
    }

    private void Start()
    {
        _useHintButton.onClick.AddListener(UseHint);
    }

    private void OnDestroy()
    {
        _useHintButton.onClick.RemoveListener(UseHint);
    }

    public void AddHint()
    {
        _hintAmount++;
        UpdateHintAmountText();
    }

    public void SubtractHint()
    {
        if (_hintAmount > 0)
        {
            _hintAmount--;
            UpdateHintAmountText();
        }
    }

    private void UpdateHintAmountText()
    {
        _hintAmountText.text = $"{_hintAmount}";
    }

    public void UseHint()
    {
        if (_hintAmount <= 0)
        {
            Debug.LogWarning("No hints available to use.");
            return;
        }

        if (_currentHintIndex < _hintData.Hints.Length)
        {
            UIManager.Instance.ShowDialog("Hint", new HintProperties(_hintData.Hints[_currentHintIndex]));
        }
        else
        {
            UIManager.Instance.ShowDialog("Hint", new HintProperties(_hintData.DefaultHint));
        }

        SubtractHint();
        if (_currentHintIndex < _hintAmount - 1)
        {
            _currentHintIndex++;
        }
        else
        {
            _adsImage.SetActive(true);
        }
    }
}
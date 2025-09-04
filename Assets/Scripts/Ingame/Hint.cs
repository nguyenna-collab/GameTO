using Service_Locator;
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

    private UserDataManager _userDataManager;
    
    public int HintAmount
    {
        get => _hintAmount;
        set
        {
            if (value < 0) value = 0;
            _hintAmount = value;
        }
    }

    private int _currentHintIndex = 0;
    private int _hintAmount;    

    void Awake()
    {
        _hintAmountText ??= GetComponentInChildren<TMP_Text>();
        _useHintButton ??= GetComponentInChildren<Button>();
    }

    private void Start()
    {
        _useHintButton.onClick.AddListener(UseHint);
        ServiceLocator.Global.Get<UserDataManager>(out _userDataManager);
        UpdateHintAmountText();
    }

    private void OnDestroy()
    {
        _useHintButton.onClick.RemoveListener(UseHint);
    }

    public void AddHint()
    {
        HintAmount++;
        UpdateHintAmountText();
    }

    public void SubtractHint()
    {
        if (HintAmount > 0)
        {
            HintAmount--;
            UpdateHintAmountText();
        }
    }

    private void UpdateHintAmountText()
    {
        _hintAmountText.text = $"{HintAmount}";
        _userDataManager.UserData.Hints = HintAmount;
        _userDataManager.Save();
    }

    public void UseHint()
    {
        if (HintAmount <= 0)
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
        if (_currentHintIndex < HintAmount - 1)
        {
            _currentHintIndex++;
        }
        else
        {
            _adsImage.SetActive(true);
        }
    }
}
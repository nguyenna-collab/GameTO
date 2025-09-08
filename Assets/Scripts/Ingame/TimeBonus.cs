using Service_Locator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBonus : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameplayTimer _gameplayTimer;
    [SerializeField] private TMP_Text _bonusText;
    [SerializeField] private Button _bonusButton;
    [SerializeField] private GameObject _adsImage;

    private SaveManager _saveManager;
    
    public int BonusAmount { get; set; }

    void Start()
    {
        ServiceLocator.Global.Get(out _saveManager);
        UpdateUI();
    }

    void OnEnable()
    {
        _bonusButton.onClick.AddListener(UseBonus);
    }

    void OnDisable()
    {
        _bonusButton.onClick.RemoveListener(UseBonus);
    }

    public void UseBonus()
    {
        _gameplayTimer.AddTime(BonusAmount);
        BonusAmount = 0;
        UpdateUI();
    }

    public void AddBonus(int amount)
    {
        BonusAmount = amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _bonusText.text = $"+{BonusAmount}s";
        if (BonusAmount <= 0)
        {
            _bonusText.gameObject.SetActive(false);
            _adsImage.SetActive(true);
        }
        else
        {
            _bonusText.gameObject.SetActive(true);
            _adsImage.SetActive(false);
        }
        UpdateData();
    }

    private void UpdateData()
    {
        _saveManager.UserData.TimeBonus = BonusAmount;
        _saveManager.SaveUserData();
    }
}
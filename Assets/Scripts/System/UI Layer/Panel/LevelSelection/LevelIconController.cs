using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class LevelIconController : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Image _hotImage;
    [SerializeField] private TMP_Text _levelText;

    private Button _btn;
    private LevelDataSO _levelData;

    void Awake()
    {
        _btn = GetComponent<Button>();
    }

    void OnEnable()
    {
        _btn.onClick.AddListener(OnButtonClick);
    }

    void OnDisable()
    {
        _btn.onClick.RemoveListener(OnButtonClick);
    }

    public void SetIconData(LevelDataSO iconData)
    {
        if (_iconImage == null || _levelText == null || iconData == null)
        {
            Debug.LogWarning("Icon image or level text is not set or iconData is null.");
            return;
        }

        _iconImage.sprite = iconData.Icon;
        _levelText.SetText($"Level {iconData.LevelIndex}");
        _levelData = iconData;
        if (_levelData.IsLocked)
            _lockedImage.gameObject.SetActive(true);
        else
            _lockedImage.gameObject.SetActive(false);
        if (_levelData.IsHotLevel)
            _hotImage.gameObject.SetActive(true);
        else
            _hotImage.gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        if (_levelData != null && !_levelData.IsLocked)
        {
            UIManager.Instance.HideAllPanels();
            UIManager.Instance.HideAllDialogs();
            SceneManagementService.Instance.LoadLevel(_levelData.LevelIndex);
        }
        else
        {
            Debug.Log("Level is locked.");
        }
    }
}
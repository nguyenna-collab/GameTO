using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class LevelIconController : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _levelText;

    private Button _btn;

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

    public void SetIconData(LevelIconDataSO iconData)
    {
        if (_iconImage == null || _levelText == null || iconData == null)
        {
            Debug.LogWarning("Icon image or level text is not set or iconData is null.");
            return;
        }

        _iconImage.sprite = iconData.Icon;
        _levelText.text = iconData.LevelIndex.ToString();
    }

    private void OnButtonClick()
    {
        SceneManagementService.Instance.LoadScene("Level_" + _levelText.text);
    }
}
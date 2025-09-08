using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour
{
    [SerializeField] private LevelDataListSO _levelIconListSO;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(HandleButtonClick);
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        UIManager.Instance.ShowPanel("LevelSelection", ScreenPropertiesFactory.CreateLevelSelectionProperties(_levelIconListSO));
    }
}
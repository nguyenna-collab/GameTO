using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackToMenuButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleBackToMenu);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleBackToMenu);
    }

    private void HandleBackToMenu()
    {
        UIManager.Instance.ShowDialog("ReturnHome", ScreenPropertiesFactory.CreateReturnHomeProperties(
            "Do you really want to quit?\nYour progress will be lost."
        ));
    }
}
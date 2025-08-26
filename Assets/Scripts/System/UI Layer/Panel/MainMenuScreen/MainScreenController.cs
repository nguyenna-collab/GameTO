using UnityEngine;
using UnityEngine.UI;

public class MainScreenController : AUIScreenController
{
    [SerializeField] private Button _playButton;

    public override ScreenProperties BaseProperties => null;

    protected override void Awake()
    {
        base.Awake();
        _playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        UIManager.Instance.ShowDialog("LevelSelection");
    }
}
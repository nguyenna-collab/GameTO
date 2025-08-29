using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelResultProperties : ScreenProperties
{
    public LevelResultProperties(Sprite levelSprite, bool isWin)
    {
        blockInput = true;
        LevelSprite = levelSprite;
        IsWin = isWin;
    }

    public Sprite LevelSprite { get; set; }
    public bool IsWin { get; set; }
}

public class LevelResultScreen : AUIScreenController<LevelResultProperties>
{
    [SerializeField] private Image _levelImage;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _replayButton;
    [SerializeField] private GameObject _doneMark;
    [SerializeField] private GameObject _failedMark;

    private Animator _anim;

    protected override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _continueButton.onClick.AddListener(OnContinueClicked);
        _replayButton.onClick.AddListener(OnReplayClicked);
    }

    void OnDisable()
    {
        _continueButton.onClick.RemoveListener(OnContinueClicked);
        _replayButton.onClick.RemoveListener(OnReplayClicked);
    }

    protected override void OnPropertiesSet()
    {
        if (Properties.IsWin)
        {
            _resultText.text = "Level Completed";
            _continueButton.gameObject.SetActive(true);
            _replayButton.gameObject.SetActive(true);
            _doneMark.SetActive(true);
            _failedMark.SetActive(false);
        }
        else
        {
            _resultText.text = "Level Failed";
            _continueButton.gameObject.SetActive(false);
            _replayButton.gameObject.SetActive(true);
            _doneMark.SetActive(false);
            _failedMark.SetActive(true);
        }
        _levelImage.sprite = Properties.LevelSprite;
    }

    public void PlayResultMarkAnimation()
    {
        if (_anim != null)
        {
            if (Properties.IsWin)
                _anim.SetTrigger("Done");
            else
                _anim.SetTrigger("Failed");
        }
    }

    private void OnContinueClicked()
    {
        SceneManagementService.Instance.LoadNextLevel();
        UIManager.Instance.HideDialog(ScreenID);
    }

    private void OnReplayClicked()
    {
        SceneManagementService.Instance.RestartLevel();
        UIManager.Instance.HideDialog(ScreenID);
    }
}
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _timerText;

    [Header("Initialization")]
    [SerializeField, Range(0, 59)] private int _minutes;
    [SerializeField, Range(0, 59)] private int _seconds;
    

    public Action OnTimerStart;
    public Action OnTimerPause;
    public Action OnTimeUp;

    private float _totalSeconds;

    private Coroutine _updateTimerCoroutine;

    void Start()
    {
        _totalSeconds = (_minutes * 60) + _seconds;
        _updateTimerCoroutine = StartCoroutine(UpdateTimerCoroutine());
    }

    void OnEnable()
    {
        OnTimeUp += HandleTimeUp;
        LevelsManager.Instance.OnCurrentLevelCompleted += Pause;
        LevelsManager.Instance.OnCurrentLevelFailed += Pause;
    }

    void OnDisable()
    {
        OnTimeUp -= HandleTimeUp;
        LevelsManager.Instance.OnCurrentLevelCompleted -= Pause;
        LevelsManager.Instance.OnCurrentLevelFailed -= Pause;
        StopCoroutine(_updateTimerCoroutine);
        _updateTimerCoroutine = null;
    }

    public void Pause()
    {
        StopCoroutine(_updateTimerCoroutine);
        OnTimerPause?.Invoke();
    }

    public void Resume()
    {
        _updateTimerCoroutine = StartCoroutine(UpdateTimerCoroutine());
    }

    private IEnumerator UpdateTimerCoroutine()
    {
        OnTimerStart?.Invoke();

        while (_totalSeconds > 0)
        {
            _totalSeconds -= Time.deltaTime;
            _timerText.text = Mathf.FloorToInt(_totalSeconds / 60).ToString("00") + ":" + Mathf.FloorToInt(_totalSeconds % 60).ToString("00");
            yield return null;
        }

        OnTimeUp?.Invoke();
    }

    private void HandleTimeUp()
    {
        _timerText.text = "00:00";
        LevelsManager.Instance.OnCurrentLevelFailed?.Invoke();
        Debug.Log("Time's up!");
    }

    public void AddTime(int bonousTime)
    {
        _totalSeconds += bonousTime;
    }
}
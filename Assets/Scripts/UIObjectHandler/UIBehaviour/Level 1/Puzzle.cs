using UnityEngine;
using UnityEngine.Events;


public class Puzzle : MonoBehaviour
{
    [SerializeField] private Objective _objective;

    [Header("When Completed")]
    public UnityEvent OnSuccess;

    private SlidingPuzzle _slidingPuzzle;

    private void Awake() {
        _slidingPuzzle = GetComponent<SlidingPuzzle>();
    }

    private void OnEnable() {
        _slidingPuzzle.OnPuzzleSolved += CompleteObjective;
    }

    private void OnDisable() {
        _slidingPuzzle.OnPuzzleSolved -= CompleteObjective;
    }

    private void CompleteObjective()
    {
        _objective.CompleteObjective();
        OnSuccess?.Invoke();
    }
}
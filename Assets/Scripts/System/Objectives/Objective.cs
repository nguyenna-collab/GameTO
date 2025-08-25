using System;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private ObjectiveSO _objectiveSO;
    [SerializeField] private ObjectiveManager _objectiveManager;

    public ObjectiveSO ObjectiveSO { get => _objectiveSO; }
    public ObjectiveManager ObjectiveManager { get => _objectiveManager; }

    public Action OnObjectiveCompleted;

    public void CompleteObjective()
    {
        _objectiveSO.CompleteObjective();
        OnObjectiveCompleted?.Invoke();
    }

    public void FailObjective()
    {
        _objectiveSO?.FailObjective();
    }
}
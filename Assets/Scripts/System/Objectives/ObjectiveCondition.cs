using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectiveCondition : MonoBehaviour
{
    [SerializeField] private Objective _targetObjective;
    [SerializeField, Tooltip("Objectives to set properties before this objective completed")] private List<Objective> _objectivesToSetPropertiesBeforeCompleted;

    [Header("Apply Properties")]
    [SerializeField] private bool _applyInteractable = false;
    [SerializeField] private bool _applyRaycastTarget = false;
    [SerializeField] private bool _canDetectTarget = false;

    [Header("Set Value")]
    [SerializeField] private bool _interactableValue = false;
    [SerializeField] private bool _raycastTargetValue = false;
    [SerializeField] private bool _canDetectTargetValue = false;

    private Objective _objective;

    private void Awake() {
        SetValuesBeforeCompleted();
    }

    private void OnEnable()
    {
        _targetObjective.OnObjectiveCompleted += HandleObjectiveCompleted;
    }
    
    private void OnDisable() {
        _targetObjective.OnObjectiveCompleted -= HandleObjectiveCompleted;
    }

    private void SetValuesBeforeCompleted()
    {
        if (_targetObjective.ObjectiveSO.IsCompleted) return;
        foreach (var objective in _objectivesToSetPropertiesBeforeCompleted)
        {
            if (_applyInteractable) objective.GetComponent<CanvasGroup>().interactable = _interactableValue;
            if (_applyRaycastTarget) objective.GetComponent<Image>().raycastTarget = _raycastTargetValue;
            if (_canDetectTarget) objective.GetComponent<AUIBehaviour>().CanDetectTarget = _canDetectTargetValue;
        }
    }

    private void HandleObjectiveCompleted()
    {
        foreach (var objective in _objectivesToSetPropertiesBeforeCompleted)
        {
            if (_applyInteractable) objective.GetComponent<CanvasGroup>().interactable = !_interactableValue;
            if (_applyRaycastTarget) objective.GetComponent<Image>().raycastTarget = !_raycastTargetValue;
            if (_canDetectTarget) objective.GetComponent<AUIBehaviour>().CanDetectTarget = !_canDetectTargetValue;
        }
    }
}
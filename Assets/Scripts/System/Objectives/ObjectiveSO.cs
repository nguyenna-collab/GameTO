using System;
using UnityEngine;
using UnityAtoms.BaseAtoms;

/// <summary>
/// Base class for a ScriptableObject-based game objective. Implement the logic within each concrete
/// class and customize the win/lose conditions.
///
/// Common objectives might include:
///
///     -Reaching a score goal (example shown here)
///     -Defeating a specific number of enemies
///     -Reaching a specific location
///     -Picking up a specific item
///     -Complete a task within a timeframe
/// </summary>

[CreateAssetMenu(fileName = "ObjectiveSO", menuName = "UIBehaviour/Create ObjectiveSO", order = 0)]
public class ObjectiveSO : DescriptionSO
{
    [Space]
    [Tooltip("On-screen name")]
    [SerializeField] private string _Title;

    //[Tooltip("Is the objective required to win")]
    //[SerializeField] private bool m_IsOptional;

    [Header("Broadcast on Event Channels")]
    [Tooltip("Event sent that objective is complete")]
    [SerializeField] private VoidEvent _ObjectiveCompleted;

    //[Tooltip("Signal that we cannot complete objective (optional)")]
    [SerializeField] private VoidEvent _ObjectiveFailed;

    [SerializeField] private bool _isCompleted;

    // Properties
    public bool IsCompleted => _isCompleted;
    public VoidEvent ObjectiveComplete => _ObjectiveCompleted;

    // Methods

    private void OnEnable()
    {
        _isCompleted = false;
    }

    // private void Awake()
    // {
    //     NullRefChecker.Validate(this);
    // }

    public virtual void CompleteObjective()
    {
        _isCompleted = true;
        _ObjectiveCompleted.Raise();
    }

    public void ResetObjective()
    {
        _isCompleted = false;
    }


    public virtual void FailObjective()
    {
        _isCompleted = false;
        _ObjectiveFailed.Raise();
    }

    public void SetComplete(bool status)
    {
        _isCompleted = status;
    }
}
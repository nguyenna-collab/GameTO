using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

/// <summary>
/// Use this to track gameplay goals or objectives. This notifies the m_AllObjectivesCompleted
/// event channel when each Objective registers as completed. This listens for notifications
/// that the game has started and that an Objective has been completed.
/// </summary>
public class ObjectiveManager : MonoBehaviour
{

    [Tooltip("List of Objectives needed for win condition.")]
    [SerializeField] private List<ObjectiveSO> _successObjectives = new();
    [SerializeField] private List<ObjectiveSO> _failureObjectives = new();
    [Header("Broadcast on Event Channels")]
    [Tooltip("Signal that all objectives are complete.")]
    [SerializeField] private VoidEvent _AllObjectivesCompleted;

    [Header("Listen to Event Channels")]
    [Tooltip("Gameplay has begun.")]
    [SerializeField] private VoidEvent _GameStarted;
    [Tooltip("Signal to update every time a single objective completes.")]
    [SerializeField] private VoidEvent _ObjectiveCompleted;
    [SerializeField] private VoidEvent _ObjectiveFailed;

    [Title("Sub ObjectiveManagers")]
    [SerializeField] private List<ObjectiveManager> _SubObjectiveManagers = new();
    [SerializeField] private VoidEvent _AllSubObjectiveManagerCompleted;
    [SerializeField] private VoidEvent _SubObjectiveManagerCompleted;

    public List<ObjectiveManager> SubObjectiveManagers { get => _SubObjectiveManagers; }

    public bool IsAllObjectivesCompleted { get; private set; }
    public bool IsFailed
    {
        get
        {
            return _isFailed;
        }
        private set
        {
            _isFailed = value;
            if (value == true)
            {
                IsAllObjectivesCompleted = false;
            }
        }
    }
    public bool IsAllSubObjectiveManagersCompleted { get; private set; }

    private bool _isFailed;

    void Awake()
    {
        Debug.Log($"{gameObject.name}: ObjectiveManager Awake");
        foreach (ObjectiveSO objective in _successObjectives)
        {
            if (objective != null)
            {
                objective.IsCompleted = false;
            }
        }

        foreach (ObjectiveSO objective in _failureObjectives)
        {
            if (objective != null)
            {
                objective.IsCompleted = false;
            }
        }
    }

    // Subscribes to event channels for starting the game and for the completion of each Objective
    private void OnEnable()
    {
        // == Objectives ==
        if (_GameStarted != null)
            _GameStarted.Register(OnGameStarted);

        if (_ObjectiveFailed != null)
            _ObjectiveFailed.Register(OnFailObjective);

        if (_ObjectiveCompleted != null)
            _ObjectiveCompleted.Register(OnCompleteObjective);

        // == Sub ObjectiveManagers
        if (_SubObjectiveManagerCompleted != null)
            _SubObjectiveManagerCompleted.Register(SubObjectiveManagerCompleted);
    }

    // Unsubscribes to prevent errors
    private void OnDisable()
    {
        if (_GameStarted != null)
            _GameStarted.Unregister(OnGameStarted);

        if (_ObjectiveCompleted != null)
            _ObjectiveCompleted.Unregister(OnCompleteObjective);

        if (_ObjectiveFailed != null)
            _ObjectiveFailed.Unregister(OnFailObjective);

        // == Sub ObjectiveManagers
        if (_SubObjectiveManagerCompleted != null)
            _SubObjectiveManagerCompleted.Unregister(SubObjectiveManagerCompleted);
    }

    #region Handle Objectives

    // Returns true if all objectives are complete
    public bool IsSuccessObjectivesListComplete()
    {
        foreach (ObjectiveSO objective in _successObjectives)
        {
            if (!objective.IsCompleted)
            {
                return false;
            }
        }

        Debug.Log($"{gameObject.name}: All Objectives completed");
        IsAllObjectivesCompleted = true;
        return true;
    }

    // Event-handling methods

    // Reset each Objective when the game begins
    private void OnGameStarted()
    {
        foreach (ObjectiveSO objective in _successObjectives)
        {
            objective.ResetObjective();
        }
    }

    // Check if all Objectives are complete every time one Objective finishes.
    // Broadcasts the m_AllObjectivesCompleted event to the GameManager, if so.
    private void OnCompleteObjective()
    {
        if (_failureObjectives != null && _failureObjectives.Count > 0)
        {
            foreach (ObjectiveSO obj in _failureObjectives)
            {
                if (obj.IsCompleted)
                {
                    _ObjectiveFailed.Raise();
                    return;
                }
            }
        }

        if (IsSuccessObjectivesListComplete())
        {
            if (_AllObjectivesCompleted != null)
                _AllObjectivesCompleted.Raise();

            if (_SubObjectiveManagerCompleted != null)
                _SubObjectiveManagerCompleted.Raise();
        }
    }

    private void OnFailObjective()
    {
        IsFailed = true;
    }

    #endregion

    #region Handle ObjectiveManager

    public bool IsSubObjectiveManagerListComplete()
    {
        if (_SubObjectiveManagers == null || _SubObjectiveManagers.Count <= 0) return false;
        foreach (ObjectiveManager om in _SubObjectiveManagers)
        {
            Debug.Log($"{om.gameObject.name}: {om.IsAllObjectivesCompleted}");
            if (!om.IsAllObjectivesCompleted)
            {
                return false;
            }
        }
        return true;
    }

    private void SubObjectiveManagerCompleted()
    {
        if (IsSubObjectiveManagerListComplete())
        {
            if (_AllSubObjectiveManagerCompleted != null)
            {
                _AllSubObjectiveManagerCompleted.Raise();
            }
            IsAllSubObjectiveManagersCompleted = true;
        }
    }

    #endregion

    public bool IsObjectiveManagerCompleted(ObjectiveManager manager)
    {
        bool match = _SubObjectiveManagers.Find(m => m == manager && m.IsAllObjectivesCompleted);
        return match;
    }
}
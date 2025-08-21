using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameSystemsCookbook
{
    /// <summary>
    /// Use this to track gameplay goals or objectives. This notifies the m_AllObjectivesCompleted
    /// event channel when each Objective registers as completed. This listens for notifications
    /// that the game has started and that an Objective has been completed.
    /// </summary>
    public class ObjectiveManager : MonoBehaviour
    {

        [Tooltip("List of Objectives needed for win condition.")]
        [SerializeField] private List<ObjectiveSO> _Objectives = new();

        [Header("Broadcast on Event Channels")]
        [Tooltip("Signal that all objectives are complete.")]
        [SerializeField] private VoidEvent _AllObjectivesCompleted;

        [Header("Listen to Event Channels")]
        [Tooltip("Gameplay has begun.")]
        [SerializeField] private VoidEvent _GameStarted;
        [Tooltip("Signal to update every time a single objective completes.")]
        [SerializeField] private VoidEvent _ObjectiveCompleted;
        
        [Title("Sub ObjectiveManagers")]
        [SerializeField] private List<ObjectiveManager> _SubObjectiveManagers = new();
        [SerializeField] private VoidEvent _AllSubObjectiveManagerCompleted;
        [SerializeField] private VoidEvent _SubObjectiveManagerCompleted;
        

        public bool IsAllObjectivesCompleted { get; private set; }

        // Subscribes to event channels for starting the game and for the completion of each Objective
        private void OnEnable()
        {
            // == Objectives ==
            if (_GameStarted != null)
                _GameStarted.Register(OnGameStarted);

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
            
            // == Sub ObjectiveManagers
            if (_SubObjectiveManagerCompleted != null)
                _SubObjectiveManagerCompleted.Unregister(SubObjectiveManagerCompleted);
        }

        #region Handle Objectives

        // Returns true if all objectives are complete
        public bool IsObjectiveListComplete()
        {
            foreach (ObjectiveSO objective in _Objectives)
            {
                if (!objective.IsCompleted)
                {
                    return false;
                }
            }

            Debug.Log("All Objectives completed");
            IsAllObjectivesCompleted = true;
            return true;
        }


        // Event-handling methods

        // Reset each Objective when the game begins
        private void OnGameStarted()
        {
            foreach (ObjectiveSO objective in _Objectives)
            {
                objective.ResetObjective();
            }
        }

        // Check if all Objectives are complete every time one Objective finishes.
        // Broadcasts the m_AllObjectivesCompleted event to the GameManager, if so.
        private void OnCompleteObjective()
        {
            if (IsObjectiveListComplete())
            {
                if (_AllObjectivesCompleted != null)
                    _AllObjectivesCompleted.Raise();
                
                if (_SubObjectiveManagerCompleted != null)
                    _SubObjectiveManagerCompleted.Raise();
            }
        }

        #endregion

        #region Handle ObjectiveManager

        public bool IsSubObjectiveManagerListComplete()
        {
            if (_SubObjectiveManagers == null || _SubObjectiveManagers.Count < 0) return false;
            foreach (ObjectiveManager om in _SubObjectiveManagers)
            {
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
                    _AllSubObjectiveManagerCompleted.Raise();
            }
        }
        
        #endregion

        public bool ObjectiveManagerIsCompleted(ObjectiveManager manager)
        {
            bool match = _SubObjectiveManagers.Find(m => m == manager && m.IsAllObjectivesCompleted);
            return match;
        }
    }
}
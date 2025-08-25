using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Level1
{
    public class OpenButtonUI : MonoBehaviour
    {
        [SerializeField] private Killer _killer;
        [SerializeField] private ObjectiveManager _superObjectiveManager;
        [SerializeField] private FloorMover _floorMover;
        [SerializeField] private Animator _topAnimator;
        
        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
            _topAnimator.enabled = false;
        }

        private void OnEnable()
        {
            _btn.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            _btn.onClick.AddListener(Click);
        }

        public void Click()
        {
            int index = _floorMover.CurrentFloorIndex;
            if (_superObjectiveManager.IsObjectiveManagerCompleted(_superObjectiveManager.SubObjectiveManagers[index]))
            {
                _killer.ObjectivesCompleted();
                if (_floorMover.IsLastFloor)
                {
                    _topAnimator.enabled = true;
                    _topAnimator.SetTrigger("Kill");
                }
            }
            else
                _killer.ObjectivesFail();
        }
    }
}
using System;
using DG.Tweening;
using GameSystemsCookbook;
using UnityEngine;
using UnityEngine.UI;

namespace Level1
{
    public class OpenButtonUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Killer _killer;
        [SerializeField] private Transform _door;
        [SerializeField] private ObjectiveManager _superObjectiveManager;
        [SerializeField] private ObjectiveManager _currentLevelObjectiveManager;
        
        [Header("Parameters")] 
        [SerializeField] private float _killerMoveRightDistance;
        [SerializeField] private float _killerMoveRightDuration;
        [SerializeField] private float _killerMoveLeftDistance;
        [SerializeField] private float _killerMoveLeftDuration;
        
        
        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _btn.onClick.AddListener(ButtonClick);
        }

        private void OnDisable()
        {
            _btn.onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            if (_superObjectiveManager.ObjectiveManagerIsCompleted(_currentLevelObjectiveManager))
                ObjectivesCompleted();
            else
                ObjectivesFail();
        }

        private void ObjectivesCompleted()
        {
            var killerTransform = _killer.transform;
            _door.gameObject.SetActive(false);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.5f);
            s.Append(killerTransform.DOMoveX(killerTransform.position.x + _killerMoveRightDistance, _killerMoveRightDuration));
            s.Append(killerTransform.DOMoveX(killerTransform.position.x -  _killerMoveLeftDistance, _killerMoveLeftDuration));
        }

        private void ObjectivesFail()
        {
            Debug.Log("Mission fail!!!");
        }
    }
}
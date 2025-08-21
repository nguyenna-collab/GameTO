using System.Collections.Generic;
using DG.Tweening;
using GameSystemsCookbook;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Level1
{
    public class OpenButtonUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Killer _killer;
        [SerializeField] private List<Transform> _doorList;
        [SerializeField] private ObjectiveManager _superObjectiveManager;
        
        [Header("Parameters")]
        [SerializeField] private Ease _easeType = Ease.Linear;
        [SerializeField] private Transform _targetRight;
        [SerializeField] private float _killerMoveRightDuration;
        [SerializeField] private Transform _targetLeft;
        [SerializeField] private float _killerMoveLeftDuration;
        [SerializeField] private Transform _targetCenter;
        [SerializeField] private float _killerMoveCenterDuration;
        
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
            int index = LevelManager.Instance.CurrentFloorIndex;
            if (_superObjectiveManager.ObjectiveManagerIsCompleted(_superObjectiveManager.SubObjectiveManagers[index]))
                ObjectivesCompleted();
            else
                ObjectivesFail();
        }

        private void ObjectivesCompleted()
        {
            var killerTransform = _killer.transform;
            var levelManager = LevelManager.Instance;
            var killerScale = killerTransform.localScale;

            if (!levelManager.IsLastFloor)
            {
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() =>
                {
                    _doorList[levelManager.CurrentFloorIndex].gameObject.SetActive(false);
                }).AppendInterval(0.5f);
                s.Append(killerTransform.DOMove(_targetRight.position, _killerMoveRightDuration).SetEase(_easeType));
                s.AppendCallback(() => killerTransform.FlipX());
                s.Append(killerTransform.DOMove(_targetLeft.position, _killerMoveLeftDuration).SetEase(_easeType));
                s.Append(levelManager.MoveToNextFloor());
                s.AppendCallback(() => killerTransform.FlipX());
                s.Append(killerTransform.DOMove(_targetCenter.position, _killerMoveCenterDuration).SetEase(_easeType)); 
            }
            else
            {
                //TODO: Last Floor Sequence
                Debug.Log("Execute last floor sequence");
                Sequence s = DOTween.Sequence();
            }
        }

        private void ObjectivesFail()
        {
            Debug.Log("Mission Fail");
        }
    }
}
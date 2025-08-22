using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Level1
{
    public class MoveButtonUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Killer _killer;
        [SerializeField] private List<Transform> _doorList;
        [SerializeField] private ObjectiveManager _superObjectiveManager;
        [SerializeField] private FloorMover _floorMover;
        
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
            int index = _floorMover.CurrentFloorIndex;
            if (_superObjectiveManager.ObjectiveManagerIsCompleted(_superObjectiveManager.SubObjectiveManagers[index]))
                ObjectivesCompleted();
            else
                ObjectivesFail();
        }

        private void ObjectivesCompleted()
        {
            var killerTransform = _killer.transform;
            var killerScale = killerTransform.localScale;

            // == Execute Action Sequence ==
            if (!_floorMover.IsLastFloor)
            {
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() =>
                {
                    _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
                });
                s.Append(killerTransform.DOMove(_targetRight.position, _killerMoveRightDuration).SetEase(_easeType));
                s.JoinCallback(() => _killer.SetWalkAnim());
                s.AppendCallback(() =>
                {
                    _killer.SetIdle2Anim();
                }).AppendInterval(1f);
                s.AppendCallback(() => killerTransform.FlipX());
                s.Append(killerTransform.DOMove(_targetLeft.position, _killerMoveLeftDuration).SetEase(_easeType));
                s.JoinCallback(() => _killer.SetWalkAnim());
                s.Append(_floorMover.MoveToNextFloor());
                s.JoinCallback(() => killerTransform.FlipX());
                s.Append(killerTransform.DOMove(_targetCenter.position, _killerMoveCenterDuration).SetEase(_easeType));
                s.AppendCallback(() => _killer.SetIdleAnim());
            }
            else
            {
                //TODO: Last Floor Sequence
                Debug.Log("Execute last floor sequence");
                LevelsManager.Instance.OnCurrentLevelCompleted?.Invoke();
            }
        }

        private void ObjectivesFail()
        {
            var killerTransform = _killer.transform;
            var killerScale = killerTransform.localScale;

            // == Execute Action Sequence ==
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                _doorList[_floorMover.CurrentFloorIndex].gameObject.SetActive(false);
            });
            s.Append(killerTransform.DOMove(_targetRight.position, _killerMoveRightDuration).SetEase(_easeType));
            s.JoinCallback(() => _killer.SetWalkAnim());
            s.AppendCallback(() =>
            {
                _killer.SetIdle2Anim();
            }).AppendInterval(0.5f);

            Debug.Log("Mission Fail");
            LevelsManager.Instance.OnCurrentLevelFailed?.Invoke();
        }
    }
}
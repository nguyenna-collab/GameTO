using System;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Level1
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private List<GameObject> _floorList = new();
        [SerializeField] private RectTransform _floorRoot;
        [SerializeField] private RectTransform _floorMask;
        
        public int CurrentFloorIndex { get; private set; }
        public bool IsLastFloor { get; private set; }

        public override void Awake()
        {
            base.Awake();
            CurrentFloorIndex = 0;
        }

        [Button("Move To Next Floor")]
        private void MoveToNextFloorButton() => MoveToNextFloor();

        public GameObject GetCurrentFloor()
        {
            return _floorList[CurrentFloorIndex];
        }
        
        public Sequence MoveToNextFloor()
        {
            if (CurrentFloorIndex < 0 || CurrentFloorIndex >= _floorList.Count - 1) return null;
            
            var currentFloorPosY = _floorRoot.anchoredPosition.y;
            var nextFloorIndex = CurrentFloorIndex + 1;
            
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                TouchManager.Instance.DisableEventSystem();
                _floorList[nextFloorIndex].gameObject.SetActive(true);
            });
            s.Append(_floorRoot.DOLocalMoveY(currentFloorPosY - _floorMask.rect.height, 2));
            s.AppendCallback(() =>
            {
                TouchManager.Instance.EnableEventSystem();
                _floorList[CurrentFloorIndex].gameObject.SetActive(false);
                CurrentFloorIndex++;
                if (CurrentFloorIndex >= _floorList.Count-1) IsLastFloor = true;
            });
            return s;
        }
        
        [Button]
        private void ResetPosition(){
            for (int i = 0; i < _floorList.Count; i++)
            {
                if (i == 0)
                {
                    _floorList[i].SetActive(true);
                }
                else
                {
                    _floorList[i].SetActive(false);
                }
            }
            _floorRoot.localPosition = new Vector3(0, 0, 0);
            CurrentFloorIndex = 0;
        }
    }
}
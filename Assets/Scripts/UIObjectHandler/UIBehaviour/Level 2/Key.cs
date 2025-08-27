using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Level2
{
    public class Key : AUIBehaviour, IDropTarget {
        [SerializeField] private TungTungBoy _tungTungBoy;
        [SerializeField] private Objective _objective;

        private DraggableUI _draggableUI;

        protected override void OnEnable()
        {
            base.OnEnable();
            _draggableUI = GetComponent<DraggableUI>();
            _draggableUI.OnDropped.AddListener(OpenDoor);
        }

        protected void OnDisable()
        {
            _draggableUI.OnDropped.RemoveListener(OpenDoor);
        }

        private void OpenDoor(PointerEventData eventData)
        {
            if (IsTouchingTarget(eventData))
                OnDropReceived(_draggableUI, eventData);
            else
                FailObjective();
        }

        private void FailObjective()
        {
            _draggableUI.RestoreToInitial();
            _objective?.FailObjective();
        }

        public void OnDropReceived(DraggableUI draggable, PointerEventData eventData)
        {
            _tungTungBoy.TriggerOpenKeyDoorAnimation();
            _objective?.CompleteObjective();
            gameObject.SetActive(false);
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages touch input in the game.
/// </summary>
namespace Managers
{
    public class TouchManager : Singleton<TouchManager>
    {
        [SerializeField] private ParticleSystem _touchPsPrefab;
        [SerializeField] private Canvas _touchCanvas;
        
        private EventSystem _eventSystem;

        public override void Awake()
        {
            base.Awake();
            _eventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
                var touchPos = touch.position;
                ParticleSystemManager.Instance.PlayParticles(_touchPsPrefab, _touchCanvas.transform, touchPos);
            }
        }

        public void EnableEventSystem()
        {
            if (_eventSystem == null)
                _eventSystem = EventSystem.current;
            _eventSystem.enabled = true;
        }
        
        public void DisableEventSystem()
        {
            if (_eventSystem == null)
                _eventSystem = EventSystem.current;
            _eventSystem.enabled = false;
        }
    }
}
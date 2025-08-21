using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class TouchManager : Singleton<TouchManager>
    {
        private EventSystem _eventSystem;

        private void Awake()
        {
            base.Awake();
            _eventSystem = EventSystem.current;
        }
        
        public void EnableEventSystem() => _eventSystem.enabled = true;
        public void DisableEventSystem() => _eventSystem.enabled = false;
    }
}
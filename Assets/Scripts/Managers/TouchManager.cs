using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages touch input in the game.
/// </summary>
namespace Managers
{
    public class TouchManager : Singleton<TouchManager>
    {
        private EventSystem _eventSystem;

        public override void Awake()
        {
            base.Awake();
            _eventSystem = EventSystem.current;
        }

        public void EnableEventSystem() => _eventSystem.enabled = true;
        public void DisableEventSystem() => _eventSystem.enabled = false;
    }
}
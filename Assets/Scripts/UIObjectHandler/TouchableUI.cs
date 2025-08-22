using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Handles touch input on UI elements.
/// </summary>
public class TouchableUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<Transform> _gameobjectsToShow;
    [Space(20), SerializeField] private List<Transform> _gameObjectsToHide;
    [Space(20)]

    public UnityEvent OnTouchClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTouchClick.Invoke();
        foreach (var go in _gameobjectsToShow)
        {
            go.gameObject.SetActive(true);
        }
        foreach (var go in _gameObjectsToHide)
        {
            go.gameObject.SetActive(false);
        }
    }
}
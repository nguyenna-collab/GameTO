using Level1;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Objective))]
public class Scroll : AUIBehaviour
{
    [SerializeField] private DraggableUI _draggableUI;
    [SerializeField] private Image _poorGirl;
    [SerializeField] private Image _mummyGirl;
    [SerializeField] private Transform _eye1;
    [SerializeField] private Transform _eye2;
    [SerializeField] private Transform _poorGirlEye1Pos;
    [SerializeField] private Transform _poorGirlEye2Pos;
    [SerializeField] private Transform _mummyGirlEye1Pos;
    [SerializeField] private Transform _mummyGirlEye2Pos;
    [SerializeField] private OpenButtonUI _openButtonUI;
    [SerializeField] private Objective _objective;

    protected override void OnEnable()
    {
        base.OnEnable();
        _draggableUI = GetComponent<DraggableUI>();
        _objective = GetComponent<Objective>();
        _draggableUI.OnDropped.AddListener(TouchGirl);
    }

    protected void OnDisable()
    {
        _draggableUI.OnDropped.RemoveListener(TouchGirl);
    }

    private void TouchGirl(PointerEventData eventData)
    {
        if (IsTouchingTarget(eventData))
            OnDropReceived(_draggableUI, eventData);
        else
            FailObjective();
    }

    private void CompleteObjective()
    {
        _poorGirl.gameObject.SetActive(false);
        _mummyGirl.gameObject.SetActive(true);
        _mummyGirl.SetNativeSize();
        if (_poorGirlEye1Pos.childCount > 0)
        {
            _eye1.transform.SetParent(_mummyGirlEye1Pos);
            _eye1.transform.localPosition = Vector3.zero;
        }
        if (_poorGirlEye2Pos.childCount > 0)
        {
            _eye2.transform.SetParent(_mummyGirlEye2Pos);
            _eye2.transform.localPosition = Vector3.zero;
        }

        _objective.CompleteObjective();
        _openButtonUI.Click();
        gameObject.SetActive(false);
    }

    private void FailObjective()
    {
        _draggableUI.RestoreToInitial();
        _objective?.FailObjective();
    }

    public void OnDropReceived(DraggableUI draggable, PointerEventData eventData)
    {
        CompleteObjective();
    }
}
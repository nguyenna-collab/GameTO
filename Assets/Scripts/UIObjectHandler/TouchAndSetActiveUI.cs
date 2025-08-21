using UnityEngine;
using UnityEngine.EventSystems;

public class TouchAndSetActiveUI : AUIBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    protected override void CompleteObjective()
    {
        throw new System.NotImplementedException();
    }

    protected override void FailObjective()
    {
        throw new System.NotImplementedException();
    }
}
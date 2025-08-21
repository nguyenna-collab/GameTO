using UnityEngine;
using UnityEngine.EventSystems;

public enum SwipeDirection { None, Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight }

public class SwipeDetectorUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    
    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private SwipeDirection _swipeDirection = SwipeDirection.None;
    
    private Vector2 startPos;
    
    public SwipeDirection SwipeDirection => _swipeDirection;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 endPos = eventData.position;
        Vector2 delta = endPos - startPos;

        if (delta.magnitude < minSwipeDistance)
            return;

        DetectSwipe(delta);
    }

    private void DetectSwipe(Vector2 delta)
    {
        Vector2 dir = delta.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360f;

        if (angle >= 337.5f || angle < 22.5f)
            _swipeDirection = SwipeDirection.Right;
        else if (angle >= 22.5f && angle < 67.5f)
            _swipeDirection = SwipeDirection.UpRight;
        else if (angle >= 67.5f && angle < 112.5f)
            _swipeDirection = SwipeDirection.Up;
        else if (angle >= 112.5f && angle < 157.5f)
            _swipeDirection = SwipeDirection.UpLeft;
        else if (angle >= 157.5f && angle < 202.5f)
            _swipeDirection = SwipeDirection.Left;
        else if (angle >= 202.5f && angle < 247.5f)
            _swipeDirection = SwipeDirection.DownLeft;
        else if (angle >= 247.5f && angle < 292.5f)
            _swipeDirection = SwipeDirection.Down;
        else if (angle >= 292.5f && angle < 337.5f)
            _swipeDirection = SwipeDirection.DownRight;
    }
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SwipeDetectorUI))]
public class Vest : MonoBehaviour, IPointerUpHandler 
{
    [SerializeField] private GameObject _octopusCover;

    private SwipeDetectorUI swipeDetector;

    private void Awake()
    {
        swipeDetector = GetComponent<SwipeDetectorUI>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (swipeDetector.SwipeDirection == SwipeDirection.UpRight)
        {
            _octopusCover.SetActive(true);
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMoveX(transform.position.x + 3f, 1f));
            s.Join(transform.DOMoveY(transform.position.y + 3f, 1f));
            s.AppendCallback(() => gameObject.SetActive(false));
            // == Rotate ==
        }
    }
}
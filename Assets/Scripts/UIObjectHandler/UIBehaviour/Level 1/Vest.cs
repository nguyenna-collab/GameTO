using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SwipeDetectorUI))]
public class Vest : MonoBehaviour, IPointerUpHandler 
{
    [SerializeField] private GameObject _octopusCover;
    [SerializeField] private Objective _objective;
    [SerializeField] private AudioClip _successSound;

    private SwipeDetectorUI swipeDetector;

    private void Awake()
    {
        swipeDetector = GetComponent<SwipeDetectorUI>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (swipeDetector.SwipeDirection == SwipeDirection.UpRight)
        {
            if (_successSound != null)
                SoundManager.Instance.PlaySFX(_successSound, default, 0.5f);
            _octopusCover.SetActive(true);
            _objective?.CompleteObjective();

            Sequence s = DOTween.Sequence();
            s.Append(transform.DOMoveX(transform.position.x + 3f, 1f));
            s.Join(transform.DOMoveY(transform.position.y + 3f, 1f));
            s.AppendCallback(() => gameObject.SetActive(false));
        }
    }
}
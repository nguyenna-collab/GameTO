using DG.Tweening;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Ease _easeType = Ease.InOutSine;
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private Transform[] _doors;

    public void OpenDoors(int index)
    {
        var door = _doors[index];
        Sequence s = DOTween.Sequence();
        s.Append(door.DOMoveY(door.position.y + 200f, _moveDuration).SetEase(_easeType));
        s.Join(door.GetComponent<CanvasGroup>().DOFade(0, _moveDuration));
        s.AppendCallback(() => door.gameObject.SetActive(false));
    }
}
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveObjectSequencer : MonoBehaviour
{
    [SerializeField] private List<Transform> _destinations;
    [SerializeField] private float _moveDuration = 1f;

    public Sequence MoveObjects()
    {
        Sequence s = DOTween.Sequence();
        foreach (var obj in _destinations)
        {
            s.Append(transform.DOMove(obj.position, _moveDuration)).SetEase(Ease.Linear);
        }
        return s;
    }
}
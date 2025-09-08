using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Doors : MonoBehaviour
{
    [SerializeField] private Ease _easeType = Ease.InOutSine;
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private Transform[] _doors;
    [SerializeField] private Transform _paperDoorTransform;
    [SerializeField] private Sprite _paperDoor1;
    [SerializeField] private Sprite _paperDoor2;
    [SerializeField] private Sprite _paperDoor3;
    [SerializeField] private Sprite _metalDoor;

    public Transform[] DoorList { get => _doors; set => _doors = value; }
    public int CurrentDoorIndex { get; private set; } = 0;
    public Transform CurrentDoorTransform { get; private set; }

    private void Start()
    {
        CurrentDoorTransform = _doors[CurrentDoorIndex];
    }

    public void DestroyDoor(int index)
    {
        var door = _doors[index];
        door.gameObject.SetActive(false);
        CurrentDoorIndex = index + 1;
        CurrentDoorTransform = _doors[CurrentDoorIndex];
    }

    public Sequence OpenDoor(int index, Action onCompleted = null)
    {
        var door = _doors[index];
        Sequence _openDoorSequence = DOTween.Sequence();
        _openDoorSequence.Append(door.DOMoveY(door.position.y + 2f, _moveDuration).SetEase(_easeType));
        _openDoorSequence.Join(door.GetComponent<CanvasGroup>().DOFade(0, _moveDuration));
        _openDoorSequence.AppendCallback(() =>
        {
            CurrentDoorIndex = index + 1;
            CurrentDoorTransform = _doors[CurrentDoorIndex];
            door.gameObject.SetActive(false);
            onCompleted?.Invoke();
        });
        return _openDoorSequence;
    }

    public void ChangePaperDoorSprite(int index)
    {
        if (index < 0 || index > 2) return;
        var image = _paperDoorTransform.GetComponent<Image>();
        switch (index)
        {
            case 0:
                image.sprite = _paperDoor1;
                break;
            case 1:
                image.sprite = _paperDoor2;
                break;
            case 2:
                image.sprite = _paperDoor3;
                break;
        }
    }

    public void ChangeDoorToMetalSprite()
    {
        var image = _paperDoorTransform.GetComponent<Image>();
        image.sprite = _metalDoor;
    }
}
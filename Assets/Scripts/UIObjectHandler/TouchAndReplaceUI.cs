using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchAndSetReplaceUI : AUIBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite _newSprite;
    [SerializeField] private bool _setNativeSize;
    [SerializeField] private Objective _objective;

    private Image _image;

    private void Awake() {
        _image ??= GetComponent<Image>();
    }

    private void CompleteObjective()
    {
        _image.color = Color.white;
        if (_setNativeSize) _image.SetNativeSize();
        _image.sprite = _newSprite;
        _objective?.CompleteObjective();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CompleteObjective();
    }
}
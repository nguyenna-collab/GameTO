using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchAndSetReplaceUI : AUIBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite _newSprite;
    [SerializeField] private bool _setNativeSize;
    [SerializeField] private ObjectiveSO _objectiveSO;

    private Image _image;

    private void Awake() {
        _image ??= GetComponent<Image>();
    }

    protected override void CompleteObjective()
    {
        _image.color = Color.white;
        if (_setNativeSize) _image.SetNativeSize();
        _image.sprite = _newSprite;
        _objectiveSO.CompleteObjective();
    }

    protected override void FailObjective()
    {
        _objectiveSO.FailObjective();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CompleteObjective();
    }
}
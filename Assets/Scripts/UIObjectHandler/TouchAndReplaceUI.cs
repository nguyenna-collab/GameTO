using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchAndSetReplaceUI : AUIBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite _newSprite;
    [SerializeField] private bool _setNativeSize;
    [SerializeField] private Objective _objective;
    [SerializeField] private AudioClip _successSound;

    private Image _image;

    private void Awake() {
        _image ??= GetComponent<Image>();
    }

    private void CompleteObjective()
    {
        if (_successSound != null)
            SoundManager.Instance.PlaySFX(_successSound, default, 0.5f);
        if (_setNativeSize)
            _image.SetNativeSize();
        _image.color = Color.white;
        _image.sprite = _newSprite;
        _objective?.CompleteObjective();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CompleteObjective();
    }
}
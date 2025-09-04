using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FoldoutGroup : MonoBehaviour
    {
        [SerializeField] private RectMask2D _mask;
        [SerializeField] private Button _foldoutButton;
        [SerializeField, Min(0.1f)] private float _duration = 0.3f;

        private Coroutine _foldGroupCoroutine;
        private Coroutine _rotateArrowCoroutine;
        private RectTransform _rectTransform;
        private RectTransform _buttonRect;
        private float _maxHeight;
        private bool _isOpen = true;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _buttonRect = _foldoutButton.GetComponent<RectTransform>();
            _maxHeight = _rectTransform.sizeDelta.y;
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _maxHeight);
            _buttonRect.localEulerAngles =  Vector3.zero;
        }

        private void OnEnable()
        {
            _foldoutButton.onClick.AddListener(OnFoldoutButtonClick);
        }

        private void OnDisable()
        {
            _foldoutButton.onClick.RemoveListener(OnFoldoutButtonClick);
            if (_foldGroupCoroutine != null)
            {
                StopCoroutine(_foldGroupCoroutine);
                StopCoroutine(_rotateArrowCoroutine);
            }
        }

        private void OnFoldoutButtonClick()
        {
            if (_foldGroupCoroutine != null) StopCoroutine(_foldGroupCoroutine);
            float duration = (_rectTransform.rect.height / _maxHeight) * _duration;
            float startHeight = _rectTransform.sizeDelta.y;
            
            var currentHeight =  _rectTransform.sizeDelta.y;
            var currentZRoation =  _buttonRect.localEulerAngles.z;

            if (_isOpen)
            {
                _foldGroupCoroutine = StartCoroutine(AnimateHeight(currentHeight, 0));
                _rotateArrowCoroutine = StartCoroutine(RotateArrow(currentZRoation, -180));
            }
            else
            {
                _foldGroupCoroutine = StartCoroutine(AnimateHeight(currentHeight, 300));
                _rotateArrowCoroutine = StartCoroutine(RotateArrow(currentZRoation, 0));
            }

            _isOpen = !_isOpen;
        }

        private IEnumerator AnimateHeight(float from, float to)
        {
            float distance = Mathf.Abs(to - from);
            float duration = (distance / _maxHeight) * _duration;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float height = Mathf.Lerp(from, to, t);
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, height);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, to);
            _foldGroupCoroutine = null;
        }

        private IEnumerator RotateArrow(float from, float to)
        {
            float amplitude = Mathf.Abs(to - from);
            float duration = (amplitude / 180) * _duration;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float rotation = Mathf.Lerp(from, to, t);
                _buttonRect.localEulerAngles = new Vector3(0, 0, rotation);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _buttonRect.localEulerAngles = new Vector3(_buttonRect.localRotation.x, _buttonRect.localRotation.y, to);
            _rotateArrowCoroutine = null;
        }
    }
}
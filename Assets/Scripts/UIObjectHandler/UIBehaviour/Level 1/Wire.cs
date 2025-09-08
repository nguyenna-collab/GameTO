using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Level2
{
    public class Wire : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private List<Transform> _gameobjectsToShow;
        [Space(20), SerializeField] private List<Transform> _gameObjectsToHide;
        [SerializeField] private AudioClip touchClip;
        [SerializeField] private Objective _objective;

        public UnityEvent OnObjectiveCompleted;

        [Space(20)]
        [SerializeField] private BubbleTeaGirl _bubbleTeaGirl;
        [SerializeField] private Transform _fan;
        [SerializeField] private ParticleSystem _fanToxicSmokePS;
        [SerializeField] private ParticleSystem _girlFartPS;
        [SerializeField] private ObjectiveSO _potatoObjective;
        [SerializeField] private TungTungBoy _tungTungBoy;

        private Coroutine _activeFan;

        public void OnPointerClick(PointerEventData eventData)
        {
            _activeFan = StartCoroutine(ActiveFan());
            if (_objective != null)
            {
                _objective.CompleteObjective();
                OnObjectiveCompleted?.Invoke();
            }
            if (_potatoObjective.IsCompleted)
            {
                _fanToxicSmokePS.gameObject.SetActive(true);
                _fanToxicSmokePS.Play();
            }
            if (touchClip != null)
            {
                SoundManager.Instance.PlaySFX(touchClip);
            }
            foreach (var go in _gameobjectsToShow)
            {
                go.gameObject.SetActive(true);
            }
            foreach (var go in _gameObjectsToHide)
            {
                go.gameObject.SetActive(false);
            }
        }

        public void BlowWithFart()
        {
            Debug.Log(_activeFan != null);
            Debug.Log(_fanToxicSmokePS.gameObject.activeSelf);
            Debug.Log(_fanToxicSmokePS.isPaused);
            if ((_activeFan != null && !_fanToxicSmokePS.gameObject.activeSelf) || (_fanToxicSmokePS.gameObject.activeSelf && _fanToxicSmokePS.isPaused))
            {
                _fanToxicSmokePS.gameObject.SetActive(true);
                _fanToxicSmokePS.Play();
            }
        }

        public IEnumerator ActiveFan()
        {
            while (true)
            {
                _fan.localEulerAngles += new Vector3(0, 0, 2);
                if (_fan.localEulerAngles.z >= 360)
                {
                    _fan.localEulerAngles = new Vector3(0, 0, 0);
                }
                yield return null;
            }
        }
    }
}
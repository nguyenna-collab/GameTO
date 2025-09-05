using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _touchPsPrefab;
    [SerializeField] private Canvas _touchCanvas;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            var touchPos = touch.position;
            ParticleSystemManager.Instance.PlayParticles(_touchPsPrefab, _touchCanvas.transform, touchPos);
        }
    }
}
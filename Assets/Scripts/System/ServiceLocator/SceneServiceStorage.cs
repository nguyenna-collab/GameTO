﻿using System.Collections.Generic;
using UnityEngine;

namespace Service_Locator
{
    using Object = UnityEngine.Object;
    
    public class SceneServiceStorage : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private List<Object> _services;
        
        private void Awake()
        {
            Debug.Log($"ServiceStorage: Start registering services for scene {gameObject.scene.name}");
            foreach (var service in _services)
            {
                ServiceLocator.ForSceneOf(this).Register(service.GetType(), service);
            }
        }

#if UNITY_EDITOR
        // [MenuItem("GameObject/ServiceLocator/Add Scene ServiceStorage")]
        // public static void CreateServiceStorage()
        // {
        //     var container = new GameObject("ServiceStorage [Scene]", typeof(SceneServiceStorage));
        // }
#endif
    }
}
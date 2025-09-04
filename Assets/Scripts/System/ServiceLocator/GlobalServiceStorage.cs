﻿using System.Collections.Generic;
using UnityEngine;

namespace Service_Locator
{
    public class GlobalServiceStorage : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private List<Object> _services;
        
        private void Awake()
        {
            foreach (var service in _services)
            {
                ServiceLocator.Global.Register(service.GetType(), service);
            }
        }

#if UNITY_EDITOR
        // [MenuItem("GameObject/ServiceLocator/Add Global ServiceStorage")]
        // public static void CreateServiceStorage()
        // {
        //     var container = new GameObject("ServiceStorage [Global]", typeof(GlobalServiceStorage));
        // }
#endif
    }
}
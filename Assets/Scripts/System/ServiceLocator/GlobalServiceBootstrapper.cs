﻿using UnityEngine;

namespace Service_Locator
{
    public class GlobalServiceBootstrapper : Bootstrapper
    {
        [SerializeField] bool donDestroyOnLoad;
        
        public override void Bootstrap()
        {
            Container.ConfigureAsGlobal(donDestroyOnLoad);
        }
    }
}
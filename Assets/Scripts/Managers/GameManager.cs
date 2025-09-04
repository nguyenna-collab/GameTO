using System;
using Service_Locator;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private UserDataManager _userDataManager;

        private void Start()
        {
            ServiceLocator.Global.Get<UserDataManager>(out _userDataManager);
        }

        private void OnApplicationQuit()
        {
            _userDataManager.Save();
        }
    }
}
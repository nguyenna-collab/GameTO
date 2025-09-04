using System;
using System.Collections.Generic;
using System.IO;
using Save;
using Service_Locator;
using UnityEditor;
using UnityEngine;

public class UserDataManager : Singleton<QuickSave>
{
    [Header("Initialize data if data is null")] 
    [SerializeField] private int _hints = 5;
    [SerializeField] private int _timeBonus = 60;
    [SerializeField] private bool _music = true;
    [SerializeField] private bool _sound = true;
    [SerializeField] private bool _vibration = true;
    
    private QuickSave _quickSave;
    public UserData UserData { get; set; }
    
    public readonly string USER_DATA = "userdata";

    public override void Awake()
    {
        base.Awake();
        ServiceLocator.Global.Register<UserDataManager>(this);
        ServiceLocator.Global.Get(out _quickSave);
        if (!File.Exists(Path.Combine(Application.persistentDataPath, USER_DATA.ToLower() + ".json")))
        {
            UserData = new(_hints, _timeBonus,  _music, _sound, _vibration);
            Save();
        }
        else
        {
            var data = Load();
            UserData = data;
        }
    }

    public UserData Load()
    {
        return _quickSave.Load<UserData>(USER_DATA);
    }

    public void Save()
    {
        _quickSave.Save(UserData, USER_DATA);
    }
}
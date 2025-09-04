using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataListSO", menuName = "Level/Create LevelDataList", order = 0)]
public class LevelDataListSO : ScriptableObject {
    public List<LevelDataSO> LevelDataList;

    public bool ResetUnlockedLevelBeforeStart = false;

    private void OnEnable()
    {
        if (ResetUnlockedLevelBeforeStart)
        {
            LevelDataList[0].IsLocked = false;
            for (int i = 1; i < LevelDataList.Count; i++)
            {
                LevelDataList[i].IsLocked = true;
            }
        }
    }
}
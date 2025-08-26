using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataListSO", menuName = "Level/Create LevelDataList", order = 0)]
public class LevelDataListSO : ScriptableObject {
    public List<LevelDataSO> LevelDataList;
}
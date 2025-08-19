using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelIconListSO", menuName = "LevelIcon/LevelIconListSO", order = 0)]
public class LevelIconListSO : ScriptableObject {
    public List<LevelIconDataSO> levelIcons;
}
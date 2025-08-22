using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "Level/Create LevelData", order = 0)]
public class LevelDataSO : ScriptableObject
{
    [MinValue(0)] public int LevelIndex;
    [PreviewField] public Sprite Icon;
    public bool IsLocked = true;
    public bool IsHotLevel = false;

    [Header("Hint Data")] public HintDataSO Hint;

    private bool _isCompleted = false;
}
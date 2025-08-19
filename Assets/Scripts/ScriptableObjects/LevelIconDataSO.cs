using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "LevelIcon/LevelIconDataSO", order = 0)]
public class LevelIconDataSO : ScriptableObject
{
    [InfoBox("Toggle To Open Level Icon Configuration")]
    public bool openConfiguration = false;
    [BoxGroup("Level Icon Config")]
    [ShowIf("openConfiguration"), MinValue(0)] public int LevelIndex;
    [BoxGroup("Level Icon Config")]
    [ShowIf("openConfiguration"), PreviewField] public Sprite Icon;
}
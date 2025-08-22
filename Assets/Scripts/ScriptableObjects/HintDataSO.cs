using UnityEngine;

[CreateAssetMenu(fileName = "HintDataSO-Level_", menuName = "GameTO/Create HintDataSO")]
public class HintDataSO : DescriptionSO
{
    public string DefaultHint = "No More Hints Available";
    public string[] Hints;
}
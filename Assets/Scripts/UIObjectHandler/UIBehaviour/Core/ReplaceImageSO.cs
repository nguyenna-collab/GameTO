using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ReplaceImageSO", menuName = "UIBehaviour/ReplaceImageSO", order = 0)]
public class ReplaceImageSO : ScriptableObject
{
    public Sprite replaceImage;
    public bool useRandom;
    [ShowIf("useRandom")] public Sprite[] randomImages;

    public void ApplyBehaviour(Image target)
    {
        target.sprite = replaceImage;

        if (randomImages.Length > 0 && useRandom)
        {
            target.sprite = randomImages[Random.Range(0, randomImages.Length)];
        }
    }
}
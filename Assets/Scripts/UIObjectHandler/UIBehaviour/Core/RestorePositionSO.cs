using UnityEngine;

[CreateAssetMenu(fileName = "RestorePositionSO", menuName = "UIBehaviour/RestorePositionSO", order = 0)]
public class RestorePositionSO : ScriptableObject {
    public void ApplyBehaviour(Transform target, Vector3 originalPosition) {
        target.position = originalPosition;
    }
}
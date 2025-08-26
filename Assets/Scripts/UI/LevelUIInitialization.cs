using UnityEngine;

public class LevelUIInitialization : MonoBehaviour {
    private void Start() {
        UIManager.Instance.ShowPanel("Gameplay", new GameplayProperties(LevelsManager.Instance.CurrentLevelData.Description));
    }
}
using Service_Locator;
using UnityEngine;

public class LevelUIInitialization : MonoBehaviour {
    private UserData _userData;

    private void Start()
    {
        if (ServiceLocator.Global.Get<SaveManager>() == null)
            Debug.Log("Error");
        var userDataManager = ServiceLocator.Global.Get<SaveManager>();
        _userData = userDataManager.UserData;
        UIManager.Instance.ShowPanel("Gameplay", 
            ScreenPropertiesFactory.CreateGameplayProperties(
                LevelsManager.Instance.CurrentLevelData.Description,
                _userData.TimeBonus,
                _userData.Hints));
    }
}
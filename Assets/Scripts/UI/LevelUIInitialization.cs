using Service_Locator;
using UnityEngine;

public class LevelUIInitialization : MonoBehaviour {
    private UserData _userData;

    private void Start()
    {
        if (ServiceLocator.Global.Get<UserDataManager>() == null)
            Debug.Log("Error");
        var userDataManager = ServiceLocator.Global.Get<UserDataManager>();
        _userData = userDataManager.UserData;
        UIManager.Instance.ShowPanel("Gameplay", 
            new GameplayProperties(
                LevelsManager.Instance.CurrentLevelData.Description,
                _userData.TimeBonus,
                _userData.Hints));
    }
}
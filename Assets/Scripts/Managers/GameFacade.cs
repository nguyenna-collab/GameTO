using UnityEngine;

namespace Managers
{
    public class GameFacade : Singleton<GameFacade>
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private LevelsManager _levelsManager;
        [SerializeField] private ParticleSystemManager _particleSystemManager;
        [SerializeField] private SceneManagementService _sceneManagementService;
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private TouchManager _touchManager;
        [SerializeField] private SaveManager _saveManager;

        private void OnEnable()
        {
            _sceneManagementService.OnLevelLoaded += OnLevelLoaded;
            _sceneManagementService.OnLevelUnLoaded += OnLevelUnloaded;
        }

        private void OnDisable()
        {
            _sceneManagementService.OnLevelLoaded -= OnLevelLoaded;
            _sceneManagementService.OnLevelUnLoaded -= OnLevelUnloaded;
        }

        private void OnLevelLoaded(string _)
        {
            if (_saveManager == null)
                Debug.Log("Error while getting SaveManager");
            var _userData = _saveManager.UserData;
            UIManager.Instance.ShowPanel("Gameplay", 
                ScreenPropertiesFactory.CreateGameplayProperties(
                    LevelsManager.Instance.CurrentLevelData.Description,
                    _userData.TimeBonus,
                    _userData.Hints));
        }
        
        private void OnLevelUnloaded(string _)
        {
            _uiManager.HideAllUI();
        }
    }
}
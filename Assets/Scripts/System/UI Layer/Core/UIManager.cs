using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [System.Serializable]
    public class UIScreenEntry
    {
        public string screenID;
        public GameObject screenPrefab;
    }

    [Header("UI Layers")]
    [SerializeField] private AUILayerController panelLayer;
    [SerializeField] private AUILayerController dialogLayer;
    [SerializeField] private AUILayerController overlayLayer;

    [Header("Screen Prefabs")]
    [SerializeField] private List<UIScreenEntry> screenEntries;
    private Dictionary<string, AUIScreenController> instantiatedScreens = new Dictionary<string, AUIScreenController>();

    public override void Awake()
    {
        base.Awake();
        // Ensure layers are assigned
        if (panelLayer == null) Debug.LogError("UIManager: Panel Layer is not assigned!");
        if (dialogLayer == null) Debug.LogError("UIManager: Dialog Layer is not assigned!");
        if (overlayLayer == null) Debug.LogWarning("UIManager: Overlay Layer is not assigned - blocking overlay features will not work!");
        RegisterAllScreensOnLayers();
    }

    // This method is now internal, called by UIManager itself
    public void RegisterScreen(string screenId, AUIScreenController screenController, AUILayerController targetLayer)
    {
        if (string.IsNullOrEmpty(screenId)) return;
        if (instantiatedScreens.ContainsKey(screenId))
        {
            Debug.LogWarning($"UIManager: Screen with ID '{screenId}' already registered. Overwriting.");
        }
        instantiatedScreens[screenId] = screenController;
        targetLayer.RegisterScreen(screenController);
    }

    // This method is now internal, called by UIManager itself
    private void UnregisterScreen(string screenId, AUILayerController targetLayer)
    {
        if (string.IsNullOrEmpty(screenId)) return;
        if (instantiatedScreens.ContainsKey(screenId))
        {
            instantiatedScreens.Remove(screenId);
            targetLayer.UnregisterScreen(instantiatedScreens[screenId]);
        }
    }

    private void RegisterAllScreensOnLayers()
    {
        foreach (var screen in panelLayer.GetComponentsInChildren<AUIScreenController>(true))
        {
            RegisterScreen(screen.ScreenID, screen, panelLayer);
        }

        foreach (var screen in dialogLayer.GetComponentsInChildren<AUIScreenController>(true))
        {
            RegisterScreen(screen.ScreenID, screen, dialogLayer);
        }

        foreach (var screen in overlayLayer.GetComponentsInChildren<AUIScreenController>(true))
        {
            RegisterScreen(screen.ScreenID, screen, overlayLayer);
        }
    }

        // Helper to get or instantiate a screen controller by ID
    public AUIScreenController GetOrCreateScreenController(string screenId, AUILayerController targetLayer)
    {
        if (instantiatedScreens.ContainsKey(screenId))
        {
            return instantiatedScreens[screenId];
        }

        // Find the prefab entry
        UIScreenEntry entry = screenEntries.Find(e => e.screenID == screenId);
        if (entry == null || entry.screenPrefab == null)
        {
            Debug.LogError($"UIManager: Screen prefab for ID '{screenId}' not found in list or is null.");
            return null;
        }

        // Instantiate the screen from prefab
        GameObject screenGO = Instantiate(entry.screenPrefab, targetLayer.transform);
        AUIScreenController screenController = screenGO.GetComponent<AUIScreenController>();

        if (screenController == null)
        {
            Debug.LogError($"UIManager: Screen prefab for ID '{screenId}' does not have an AUIScreenController component.");
            Destroy(screenGO);
            return null;
        }

        // Ensure the screenID matches the entry's screenID
        screenController.ScreenID = screenId;

        RegisterScreen(screenId, screenController, targetLayer);
        return screenController;
    }

        // Public methods to show/hide screens via layers
        public void ShowPanel(string screenId, object properties = null)
        {
            Debug.Log($"UIManager: Showing panel with ID '{screenId}'");
            if (panelLayer != null)
            {
                AUIScreenController screen = GetOrCreateScreenController(screenId, panelLayer);
                if (screen != null)
                {
                    // Handle properties validation
                    if (properties is ScreenProperties screenProps)
                    {
                        if (!screenProps.Validate())
                        {
                            Debug.LogError($"UIManager: Invalid properties for screen '{screenId}': {screenProps.GetSummary()}");
                            return;
                        }

                        // Handle special properties
                        if (screenProps.blockInput)
                        {
                            ShowBlockingOverlay();
                        }

                        if (screenProps.showLoadingOverlay)
                        {
                            ShowOverlay("LoadingOverlay");
                        }
                    }

                    panelLayer.ShowScreen(screen, properties);
                }
            }
        }

    //For Button Event
    public void ShowPanel(string screenId)
    {
        if (panelLayer != null)
        {
            AUIScreenController screen = GetOrCreateScreenController(screenId, panelLayer);
            panelLayer.ShowScreen(screen);
        }
    }

    public void HidePanel(string screenId)
    {
        if (panelLayer != null)
        {
            panelLayer.HideScreen(screenId);
            Debug.Log($"UIManager: Hiding panel with ID '{screenId}'");
        }
    }

    public void ShowDialog(string screenId, object properties = null)
    {
        Debug.Log($"UIManager: Showing dialog with ID '{screenId}'");
        if (properties != null)
        {
            Debug.Log($"UIManager: Properties: {properties}");
        }

        if (dialogLayer != null)
        {
            AUIScreenController screen = GetOrCreateScreenController(screenId, dialogLayer);
            if (screen != null)
            {
                // Handle properties validation
                if (properties is ScreenProperties screenProps)
                {
                    if (!screenProps.Validate())
                    {
                        Debug.LogError($"UIManager: Invalid properties for dialog '{screenId}': {screenProps.GetSummary()}");
                        return;
                    }

                    // Handle special properties
                    if (screenProps.blockInput)
                    {
                        Debug.Log("UIManager: Showing blocking overlay for dialog");
                        ShowBlockingOverlay();
                    }
                }

                dialogLayer.ShowScreen(screen, properties);
            }
        }
    }

    public void HideDialog(string screenId)
    {
        if (dialogLayer != null)
        {
            // Check if we need to hide the blocking overlay
            if (instantiatedScreens.TryGetValue(screenId, out AUIScreenController screen))
            {
                if (screen.BaseProperties != null && screen.BaseProperties.blockInput)
                {
                    HideBlockingOverlay();
                }
            }

            dialogLayer.HideScreen(screenId);
        }
    }

    public void HideAllPanels()
    {
        if (panelLayer != null) panelLayer.HideAll();
    }

    public void HideAllDialogs()
    {
        if (dialogLayer != null) dialogLayer.HideAll();
    }

    public void HideAllUI()
    {
        if (panelLayer != null) panelLayer.HideAll();
        if (dialogLayer != null) dialogLayer.HideAll();
        if (overlayLayer != null) overlayLayer.HideAll();
    }

    // Overlay Layer methods
    public void ShowOverlay(string screenId, object properties = null)
    {
        if (overlayLayer != null)
        {
            AUIScreenController screen = GetOrCreateScreenController(screenId, overlayLayer);
            if (screen != null)
            {
                overlayLayer.ShowScreen(screen, properties);
            }
        }
    }

    public void HideOverlay(string screenId)
    {
        if (overlayLayer != null)
        {
            overlayLayer.HideScreen(screenId);
        }
    }

    public void ShowBlockingOverlay()
    {
        if (overlayLayer != null && overlayLayer is OverlayLayerController overlayController)
        {
            overlayController.ShowBlockingOverlay();
        }
    }

    public void HideBlockingOverlay()
    {
        if (overlayLayer != null && overlayLayer is OverlayLayerController overlayController)
        {
            overlayController.HideBlockingOverlay();
        }
    }
    
    [Button]
    public void LogAllRegisteredScreens()
    {
        Debug.Log("UIManager: Registered Screens:");
        foreach (var entry in instantiatedScreens)
        {
            Debug.Log($"- {entry.Key}: {entry.Value.name}");
        }
    }
}

internal class CinemachineCamera
{
}
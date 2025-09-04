using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class AUILayerController : MonoBehaviour
{
    protected Dictionary<string, AUIScreenController> screens = new Dictionary<string, AUIScreenController>();

    public Dictionary<string, AUIScreenController> Screens => screens;
    
    public virtual void RegisterScreen(AUIScreenController screen)
    {
        if (!screens.ContainsKey(screen.ScreenID))
        {
            screens.Add(screen.ScreenID, screen);
            screen.transform.SetParent(transform, false);
        }
    }

    public virtual void UnregisterScreen(AUIScreenController screen)
    {
        if (screens.ContainsKey(screen.ScreenID))
        {
            screens.Remove(screen.ScreenID);
        }
    }

    public virtual void ShowScreen(AUIScreenController screen, object properties = null)
    {
        if (screen == null) return;

        screen.gameObject.SetActive(true);

        if (properties != null)
        {
            var screenType = screen.GetType();
            var setPropertiesMethod = screenType.GetMethod("SetProperties");

            if (setPropertiesMethod != null)
            {
                var propertiesType = setPropertiesMethod.GetParameters()[0].ParameterType;
                if (properties.GetType().IsAssignableFrom(propertiesType))
                {
                    setPropertiesMethod.Invoke(screen, new[] { properties });
                }
                else
                {
                    Debug.LogError($"Properties of type {properties.GetType()} are not assignable to screen {screen.ScreenID} which requires {propertiesType}.");
                }
            }
        }

        screen.Show();
    }

    public virtual void HideScreen(string screenId)
    {
        if (screens.ContainsKey(screenId))
        {
            var screen = screens[screenId];
            screen.Hide();
        }
    }
    
    public virtual void HideAll()
    {
        foreach (var screen in screens.Values)
        {
            screen.Hide();
        }
    }

    [Button]
    private void LogAllScreens()
    {
        foreach (var screen in screens)
        {
            Debug.Log($"Screen ID: {screen.Key}, Active: {screen.Value.IsVisible}");
        }
    }
}
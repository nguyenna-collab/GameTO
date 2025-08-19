using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelController : MonoBehaviour
{
    [SerializeField] private List<IBeforeSceneLoad> beforeSceneLoadHandlers = new();
    [SerializeField] private List<IAfterSceneLoad> afterSceneLoadHandlers = new();

    async void Start()
    {
        foreach (var handler in beforeSceneLoadHandlers)
        {
            await handler.OnBeforeSceneLoad();
        }
    }
}
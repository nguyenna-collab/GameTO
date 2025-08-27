using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UITouchChecker : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public Image targetUI;

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // Hoặc touch input
        {
            if (IsTopMostUI(targetUI, Input.mousePosition))
                Debug.Log("Touch đúng vào UI này và không bị che!");
        }
    }

    bool IsTopMostUI(Image target, Vector2 screenPos)
    {
        PointerEventData pointer = new PointerEventData(eventSystem);
        pointer.position = screenPos;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointer, results);

        if (results.Count == 0) return false;

        // Phần tử đầu tiên là UI trên cùng
        return results[0].gameObject == target.gameObject;
    }
}

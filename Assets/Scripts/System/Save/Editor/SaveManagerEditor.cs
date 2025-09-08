using System;
using System.Diagnostics;
using Save;
using UnityEditor;
using UnityEngine;

namespace Bap.Save
{
    [CustomEditor(typeof(SaveManager))]
    [CanEditMultipleObjects]
    public class SaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = (SaveManager)target;

            if (GUILayout.Button("Open in File Explorer"))
            {
                Process.Start(new ProcessStartInfo(Application.persistentDataPath));
            }
            
            if (GUILayout.Button("Save All Files"))
            {
                manager.SaveAll();
            }

            if (GUILayout.Button("Delete All Files"))
            {
                if (EditorUtility.DisplayDialog("Alert !!!", "Do you want to delete all save files", "Yes", "No"))
                {
                    manager.DeleteAll();
                }
            }
        }
    }
}
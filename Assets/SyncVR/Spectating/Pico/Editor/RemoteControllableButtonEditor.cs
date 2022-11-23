using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(RemoteControllableButton))]
public class RemoteControllableButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        RemoteControllableButton targetButton = (RemoteControllableButton)target;
        targetButton.isRemoteControllable = (bool)EditorGUILayout.Toggle("Is Remote Controllable", targetButton.isRemoteControllable);

        targetButton.buttonText = (Text)EditorGUILayout.ObjectField("Button Text", targetButton.buttonText, typeof(Text), true);
        targetButton.buttonCategory = (Text)EditorGUILayout.ObjectField("Category Text", targetButton.buttonCategory, typeof(Text), true);
 
        if (GUI.changed)
        {
            EditorUtility.SetDirty(targetButton);
            EditorSceneManager.MarkSceneDirty(targetButton.gameObject.scene);
        }

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }

}
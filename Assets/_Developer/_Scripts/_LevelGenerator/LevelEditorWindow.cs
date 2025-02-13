using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR

public class LevelEditorWindow : EditorWindow
{
    private Transform brickParent;
    private int levelIndex = 1;
    private string levelName = "level1";
    private string customPath = "Assets/_Developer/_Levels"; // Save location

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);

        brickParent = (Transform)EditorGUILayout.ObjectField("Brick Parent", brickParent, typeof(Transform), true);
        levelIndex = EditorGUILayout.IntField("Level Index", levelIndex);

        if (GUILayout.Button("Save Level"))
        {
            SaveLevel();
        }
    }

    private void SaveLevel()
    {
        levelName = "level" + levelIndex.ToString();
        if (brickParent == null)
        {
            Debug.LogError("Brick Parent is not assigned!");
            return;
        }

        LevelData levelData = new LevelData();

        foreach (Transform brick in brickParent)
        {
            BrickEntry entry = new BrickEntry
            {
                position = brick.position,
                objectTag = brick.tag,
            };
            levelData.bricks.Add(entry);
        }

        // Ensure directory exists
        if (!Directory.Exists(customPath))
        {
            Directory.CreateDirectory(customPath);
        }

        // Save JSON
        string filePath = Path.Combine(customPath, $"{levelName}.json");
        File.WriteAllText(filePath, JsonUtility.ToJson(levelData, true));
#if UNITY_EDITOR
        // Refresh Unity to detect the new file
        AssetDatabase.Refresh();
 #endif

        Debug.Log($"Level saved at: {filePath}");
    }

}
#endif

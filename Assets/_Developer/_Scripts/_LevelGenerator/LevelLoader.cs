using UnityEngine;
using System.IO;
using System.Collections.Generic;


public class LevelLoader : MonoBehaviour
{
    [SerializeField]private List<string> availableLevels = new List<string>();

    private string customPath = "Resources/Levels"; // Levels folder
    public List<GameObject> brickPrefabs; // Assign prefabs in Inspector

    #region Monobehaviour Callbacks

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChange -= GameManager_OnGameStateChange;
    }


    private void Start()
    {
        LoadAvailableLevels();
    }

    #endregion
    //

    //Handles Level Genrate
    public void LoadLevel(int levelIndex)
    {
        GameManager.Instance.SetCurrentLevelIndex(levelIndex);

        string levelName = $"Level/level{levelIndex}"; // Remove .json extension
        TextAsset jsonFile = Resources.Load<TextAsset>(levelName);

        if (jsonFile == null)
        {
            Debug.LogError($"Level file not found in Resources: {levelName}");
            return;
        }

        string json = jsonFile.text; // Read JSON data
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        foreach (BrickEntry brick in levelData.bricks)
        {
            GameObject prefab = FindPrefabByName(brick.objectTag);
            if (prefab == null)
            {
                Debug.LogError($"Prefab '{brick.objectTag}' not found!");
                continue;
            }

            Instantiate(prefab, brick.position, Quaternion.identity, transform);
        }

        Debug.Log($"Loaded level: {levelName}");
        GameManager.Instance.UpdateGameState(GameState.PreGame);
    }


    #region Private Methods

    private void LoadAvailableLevels()
    {
        availableLevels.Clear();

        if (Directory.Exists(customPath))
        {
            string[] files = Directory.GetFiles(customPath, "*.json");
            foreach (string file in files)
            {
                availableLevels.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        else
        {
            Debug.LogError($"Level directory not found: {customPath}");
        }
    }



    private GameObject FindPrefabByName(string prefabName)
    {
        foreach (GameObject prefab in brickPrefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null;
    }

    private void GameManager_OnGameStateChange(GameState gameState)
    {
        if(gameState == GameState.Reset)
        {
            ClearLevelObjects();

            LoadLevel(GameManager.Instance.GetCurrentLevelIndex());
        }
        
        if(gameState == GameState.LevelSelect)
        {
            ClearLevelObjects();
        }
    }

    private void ClearLevelObjects()
    {
        foreach(Transform bricks in this.transform)
        {
            Destroy(bricks.gameObject);
        }
    }

    #endregion
}

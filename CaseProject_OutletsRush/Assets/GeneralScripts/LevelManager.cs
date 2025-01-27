using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject loadedLevel;

    [SerializeField] private List<LevelScriptable> levelList;
    protected override void Awake()
    {
        base.Awake();
        InitializeGame();
    }

    private void InitializeGame()
    {
        LevelLoad();
    }

    void LevelLoad()
    {
        Globals.LevelCount = levelList.Count;
        if (PlayerPrefs.GetInt("level") != 0)
        {
            Debug.Log(PlayerPrefs.GetInt("level"));
            Globals.currentLevelIndex = PlayerPrefs.GetInt("level");

        }
        if (levelList[Globals.currentLevelIndex]._levelPrefab != null)
        {
            loadedLevel = Instantiate(levelList[Globals.currentLevelIndex]._levelPrefab, transform.position, Quaternion.identity);
        }
    }
}

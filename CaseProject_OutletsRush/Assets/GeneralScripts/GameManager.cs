using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public LevelManager lvlManager;
    public UIManager ui;

    float _timeScale;
    protected override void Awake()
    {
        base.Awake();
        InitializeGame();
    }

    private void InitializeGame()
    {
        _timeScale = Time.timeScale;
        Globals.moneyAmount = PlayerPrefs.GetInt("money");
        InitConnections();
    }
    void InitConnections()
    {
        ui.OnLevelStart += OnLevelStart;
        ui.OnNextLevel += OnNextLevel;
        ui.OnLevelRestart += OnLevelRestart;
        ui.OnGamePaused += OnLevelPause;
        ui.OnGameResume += OnLevelResume;
    }

    void OnLevelStart()
    {
        ui.startCanvas.SetActive(false);
    }
    void OnNextLevel()
    {

        Globals.currentLevelIndex++;
        if (Globals.LevelCount > Globals.currentLevelIndex)
        {
            PlayerPrefs.SetInt("level", Globals.currentLevelIndex);
        }
        else
        {
            Globals.currentLevelIndex = Random.Range(0, Globals.LevelCount - 1);
            PlayerPrefs.SetInt("level", Globals.currentLevelIndex);
        }


        int levelIndex = PlayerPrefs.GetInt("levelIndex");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnLevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnLevelPause()
    {
        Time.timeScale = 0;
    }
    public void OnLevelResume()
    {
        Time.timeScale = _timeScale;
    }

    public void MoneyUpdate(int miktar)
    {
        ui.UpdateMoney(miktar);
    }
}

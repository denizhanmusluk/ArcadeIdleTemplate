using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    public event Action OnLevelStart, OnNextLevel, OnLevelRestart, OnGamePaused, OnGameResume;
    public GameObject startCanvas;


    [SerializeField] private GameObject moneyPanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    private Vector3 initialMoneyPanelScale;


    #region Handler Functions

    public void StartLevelButton()
    {
        OnLevelStart?.Invoke();
    }
    public void NextLevelButton()
    {
        OnNextLevel?.Invoke();
    }
    public void PauseLevelButton()
    {
        OnGamePaused?.Invoke();
    }
    public void ResumeLevelButton()
    {
        OnGameResume?.Invoke();
    }
    public void RestartLevelButton()
    {
        OnLevelRestart?.Invoke();
    }
    #endregion


    private void Start()
    {
        initialMoneyPanelScale = moneyPanel.transform.localScale;

        Globals.moneyAmount = PlayerPrefs.GetInt("money");
        moneyText.text = CoefficientTransformation.Converter(Globals.moneyAmount);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateMoney(1000);
        }
    }
    public void UpdateMoney(int amount)
    {
        float oldAmount = Globals.moneyAmount;
        Globals.moneyAmount += amount;
        AnimateMoneyChange(oldAmount, Globals.moneyAmount);

        PlayerPrefs.SetInt("money", Globals.moneyAmount);
        UpgradeManager.Instance.IsEnoughMoney();
    }

    private void AnimateMoneyChange(float oldAmount, float newAmount)
    {
        float duration = 1f;
        DOTween.To(() => oldAmount, x => {
            oldAmount = x;
            moneyText.text = CoefficientTransformation.Converter((int)oldAmount);
        }, newAmount, duration)
        .SetEase(Ease.OutQuad)
        .OnUpdate(() =>
        {
            AnimateMoneyPanelScale(1f, 1.1f, 0.15f, Ease.OutElastic);
        })
        .OnComplete(() =>
        {
            moneyText.text = CoefficientTransformation.Converter(Globals.moneyAmount);
            ResetMoneyPanelScale();
        });
    }

    private void AnimateMoneyPanelScale(float startScale, float endScale, float duration, Ease easeType)
    {
        moneyPanel.transform.DOScale(initialMoneyPanelScale * startScale, duration / 2)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                moneyPanel.transform.DOScale(initialMoneyPanelScale * endScale, duration / 2).SetEase(easeType);
            });
    }

    private void ResetMoneyPanelScale()
    {
        moneyPanel.transform.DOScale(initialMoneyPanelScale, 0.2f).SetEase(Ease.OutQuad);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance = null;
    public static UpgradeManager Instance => _instance;
    public List<CharacterUpgradeSettings> _upgradeSettingsList;
    public CharacterUpgradeSettings _upgradeSettings;

    [SerializeField] UpgradeButton button_CharacterSpeedLevel, button_CharacterStackLevel;
    [SerializeField] UpgradeButton button_WorkerSpeedLevel, button_WorkerStackLevel;

    public GameObject buttonGO;
    public GameObject upgradePanel;

    private void Awake()
    {
        _instance = this;

        _upgradeSettings = _upgradeSettingsList[0];
        Globals.characterSpeedLevel = PlayerPrefs.GetInt("characterSpeedLevel", 0);
        Globals.stackCapacityLevel = PlayerPrefs.GetInt("stackCapacityLevel", 0);
        Globals.workerMoveSpeedLevel = PlayerPrefs.GetInt("workerMoveSpeedLevel", 0);
        Globals.workerStackLevel = PlayerPrefs.GetInt("workerStackLevel", 0);
    }
    private void Start()
    {
        IsEnoughMoney();
        InitButtonValues();
    }
    void InitButtonValues()
    {
        button_CharacterStackLevel.UpgradeValueInit(_upgradeSettings);
        button_CharacterSpeedLevel.UpgradeValueInit(_upgradeSettings);
        button_WorkerSpeedLevel.UpgradeValueInit(_upgradeSettings);
        button_WorkerStackLevel.UpgradeValueInit(_upgradeSettings);
    }
    public void IsEnoughMoney()
    {
        button_CharacterStackLevel.MoneyEnoughCheck(_upgradeSettings);
        button_CharacterSpeedLevel.MoneyEnoughCheck(_upgradeSettings);
        button_WorkerSpeedLevel.MoneyEnoughCheck(_upgradeSettings);
        button_WorkerStackLevel.MoneyEnoughCheck(_upgradeSettings);
    }
    public void MoveSpeedUpgradeButton()
    {
        if (Globals.characterSpeedLevel < _upgradeSettings.characterSpeed.Length - 1)
        {
            int characterMoveSpeedCost = _upgradeSettings.characterSpeedCost[Globals.characterSpeedLevel + 1];

            if (Globals.moneyAmount >= characterMoveSpeedCost)
            {

                GameManager.Instance.MoneyUpdate(-characterMoveSpeedCost);
                Globals.characterSpeedLevel++;
                PlayerPrefs.SetInt("characterSpeedLevel", Globals.characterSpeedLevel);
                IsEnoughMoney();
                InitButtonValues();
            }
        }
    }
    public void StackCapacityUpgradeButton()
    {
        if (Globals.stackCapacityLevel < _upgradeSettings.stackCapacity.Length - 1)
        {
            int characterCapacityCost = _upgradeSettings.stackCapacityCost[Globals.stackCapacityLevel + 1];

            if (Globals.moneyAmount >= characterCapacityCost)
            {

                GameManager.Instance.MoneyUpdate(-characterCapacityCost);
                Globals.stackCapacityLevel++;
                PlayerPrefs.SetInt("stackCapacityLevel", Globals.stackCapacityLevel);
                IsEnoughMoney();
                InitButtonValues();
            }

        }
    }





    public void WorkerMoveSpeedUpgrade()
    {
        if (Globals.workerMoveSpeedLevel < _upgradeSettings.workerSpeed.Length - 1)
        {
            int workerMoveSpeedCost = _upgradeSettings.characterSpeedCost[Globals.workerMoveSpeedLevel + 1];

            if (Globals.moneyAmount >= workerMoveSpeedCost)
            {
                GameManager.Instance.MoneyUpdate(-workerMoveSpeedCost);
                Globals.workerMoveSpeedLevel++;
                PlayerPrefs.SetInt("workerMoveSpeedLevel", Globals.workerMoveSpeedLevel);
                IsEnoughMoney();
                InitButtonValues();
            }
        }
    }


    public void WorkerStackUpgrade()
    {
        if (Globals.workerStackLevel < _upgradeSettings.workerStackCapacity.Length - 1)
        {
            int workerStackCost = _upgradeSettings.workerStackCapacityCost[Globals.workerStackLevel + 1];

            if (Globals.moneyAmount >= workerStackCost)
            {
                GameManager.Instance.MoneyUpdate(-workerStackCost);
                Globals.workerStackLevel++;
                PlayerPrefs.SetInt("workerStackLevel", Globals.workerStackLevel);
                IsEnoughMoney();
                InitButtonValues();
            }
        }
    }


    public void OpenPanel()
    {
        buttonGO.SetActive(false);
        upgradePanel.SetActive(true);
    }
    public void ClosePanel()
    {
        buttonGO.SetActive(true);
        upgradePanel.SetActive(false);
    }
}

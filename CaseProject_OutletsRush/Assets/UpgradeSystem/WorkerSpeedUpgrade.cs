using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorkerSpeedUpgrade : UpgradeButton
{
    public override void MoneyEnoughCheck(CharacterUpgradeSettings _upgradeSettings)
    {
        if (Globals.workerMoveSpeedLevel < _upgradeSettings.workerSpeed.Length - 1 && Globals.moneyAmount >= _upgradeSettings.workerSpeedCost[Globals.workerMoveSpeedLevel + 1])
        {
            moneyButton.interactable = true;
        }
        else
        {
            moneyButton.interactable = false;
        }
    }
    public override void UpgradeValueInit(CharacterUpgradeSettings _upgradeSettings)
    {
        int currentLvl = Globals.workerMoveSpeedLevel;
        int maxLevel = _upgradeSettings.workerSpeed.Length - 1;
        string upgradeName = _upgradeSettings.workerSpeed_UpgradeName[Globals.workerMoveSpeedLevel];

        levelText.text = currentLvl.ToString();
        levelMaxText.text = "/" + maxLevel.ToString();
        barFill.fillAmount = (float)currentLvl / (float)maxLevel;
        upgradeNameText.text = upgradeName;

        if (currentLvl < maxLevel)
        {
            int cost = _upgradeSettings.workerSpeedCost[Globals.workerMoveSpeedLevel + 1];
            if (cost == 0)
            {
                costText.text = "FREE";
            }
            else
            {
                costText.text = "$" + CoefficientTransformation.Converter(cost);
            }
        }
        else
        {
            costText.text = "MAX";
            moneyIMG.gameObject.SetActive(false);
        }
    }
}

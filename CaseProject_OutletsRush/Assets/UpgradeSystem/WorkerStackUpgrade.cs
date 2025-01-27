using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorkerStackUpgrade : UpgradeButton
{
    public override void MoneyEnoughCheck(CharacterUpgradeSettings _upgradeSettings)
    {
        if (Globals.workerStackLevel < _upgradeSettings.workerStackCapacity.Length - 1 && Globals.moneyAmount >= _upgradeSettings.workerStackCapacityCost[Globals.workerStackLevel + 1])
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
        int currentLvl = Globals.workerStackLevel;
        int maxLevel = _upgradeSettings.workerStackCapacity.Length - 1;
        string upgradeName = _upgradeSettings.workerCapacity_UpgradeName[Globals.workerStackLevel];

        levelText.text = currentLvl.ToString();
        levelMaxText.text = "/" + maxLevel.ToString();
        barFill.fillAmount = (float)currentLvl / (float)maxLevel;
        upgradeNameText.text = upgradeName;

        if (currentLvl < maxLevel)
        {
            int cost = _upgradeSettings.workerStackCapacityCost[Globals.workerStackLevel + 1];
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

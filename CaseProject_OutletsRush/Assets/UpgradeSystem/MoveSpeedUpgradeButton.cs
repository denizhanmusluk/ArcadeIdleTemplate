using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class MoveSpeedUpgradeButton : UpgradeButton
{
    public override void MoneyEnoughCheck(CharacterUpgradeSettings _upgradeSettings)
    {
        if (Globals.characterSpeedLevel < _upgradeSettings.characterSpeed.Length - 1 && Globals.moneyAmount >= _upgradeSettings.characterSpeedCost[Globals.characterSpeedLevel + 1])
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
        int currentLvl = Globals.characterSpeedLevel;
        int maxLevel = _upgradeSettings.characterSpeed.Length - 1;
        string upgradeName = _upgradeSettings.characterSpeed_UpgradeName[Globals.characterSpeedLevel];

        levelText.text = currentLvl.ToString();
        levelMaxText.text = "/" + maxLevel.ToString();
        barFill.fillAmount = (float)currentLvl / (float)maxLevel;
        upgradeNameText.text = upgradeName;

        if (currentLvl < maxLevel)
        {
            int cost = _upgradeSettings.characterSpeedCost[Globals.characterSpeedLevel + 1];
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

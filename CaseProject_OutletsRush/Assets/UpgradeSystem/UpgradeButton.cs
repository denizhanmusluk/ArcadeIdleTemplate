using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UpgradeButton : MonoBehaviour
{
    public Button moneyButton;

    public Image moneyIMG;
    public Image barFill;

    public TextMeshProUGUI levelText, costText, levelMaxText, upgradeNameText;
    public abstract void MoneyEnoughCheck(CharacterUpgradeSettings _upgradeSettings);
    public abstract void UpgradeValueInit(CharacterUpgradeSettings _upgradeSettings);
}

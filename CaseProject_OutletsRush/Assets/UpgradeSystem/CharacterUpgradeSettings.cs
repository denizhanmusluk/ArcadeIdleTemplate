using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Upgrade System/CharacterUpgradeSettings")]
public class CharacterUpgradeSettings : ScriptableObject
{
    [Header("       CHARACTER SPEED")]
    public float[] characterSpeed;
    public int[] characterSpeedCost;
    public string[] characterSpeed_UpgradeName;
    [Header("       STACK CAPACITY")]
    public int[] stackCapacity;
    public int[] stackCapacityCost;
    public string[] characterCapacity_UpgradeName;


    [Header("       WORKER SPEED")]
    public float[] workerSpeed;
    public int[] workerSpeedCost;
    public string[] workerSpeed_UpgradeName;
    [Header("       WORKER STACK CAPACITY")]
    public int[] workerStackCapacity;
    public int[] workerStackCapacityCost;
    public string[] workerCapacity_UpgradeName;
}

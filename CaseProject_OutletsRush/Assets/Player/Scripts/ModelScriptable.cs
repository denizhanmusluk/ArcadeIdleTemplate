using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Character/ModelScriptable")]

public class ModelScriptable : ScriptableObject
{

    [Header("NEW Models")]

    [SerializeField] private GameObject[] skins;
    public GameObject[] _skins { get { return skins; } }
}
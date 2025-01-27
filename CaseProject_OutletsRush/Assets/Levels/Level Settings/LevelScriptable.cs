using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelSet")]
public class LevelScriptable : ScriptableObject
{
    [Header("LevelPrefab")]
    [SerializeField] private GameObject levelPrefab;
    public GameObject _levelPrefab { get { return levelPrefab; } }

    [Header("SkyBox Material")]

    [SerializeField] private Material skyboxMaterial;
    public Material _skyboxMaterial { get { return skyboxMaterial; } }

    [Header("FogColor")]

    [SerializeField] private Color fogColor;
    public Color _fogColor { get { return fogColor; } }
}

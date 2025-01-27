using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZ_Pooling;
public enum CollectType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5,
    Type6,
    All,
    Null
}
public class Collectable : MonoBehaviour
{
    public CollectType collectType;
    public int collectID;
    public int price;


    public bool productCollectActive = false;
    public bool isCollected = false;
    public bool stackFollowingActive = false;

    private OpenElastic openElastic;


    [HideInInspector] public List<Collectable> collectedList;

    private void Start()
    {
        if (GetComponent<OpenElastic>() != null)
        {
            openElastic = GetComponent<OpenElastic>();
        }
    }
    public void ScaleEffect()
    {
        if (openElastic != null)
        {
            openElastic.OpenScale(0.8f, 1f, 0.6f, Ease.OutElastic);
        }
    }

    public void DeSpawn()
    {
        productCollectActive = false;
        isCollected = false;
        stackFollowingActive = false;
        EZ_PoolManager.Despawn(this.transform);
    }
}

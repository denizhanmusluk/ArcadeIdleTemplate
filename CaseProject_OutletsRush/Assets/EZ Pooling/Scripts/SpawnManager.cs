using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ_Pooling;

public class SpawnManager : Singleton<SpawnManager>
{
    public Collectable SpawnColectables(Transform collectableTR, Vector3 pos, Quaternion rot)
    {
        Collectable _collectable = EZ_PoolManager.Spawn(collectableTR, pos, rot).GetComponent<Collectable>();
        return _collectable;
    }

    public AIMarketCustomer SpawnCustomer(Transform cutomerTR, Vector3 pos, Quaternion rot)
    {
        AIMarketCustomer aiCustomer = EZ_PoolManager.Spawn(cutomerTR, pos, rot).GetComponent<AIMarketCustomer>();
        return aiCustomer;

    }

    public BanknotMoney SpawnMoney(Transform moneyTR, Vector3 pos, Quaternion rot)
    {
        BanknotMoney _money = EZ_PoolManager.Spawn(moneyTR, pos, rot).GetComponent<BanknotMoney>();
        return _money;

    }
}
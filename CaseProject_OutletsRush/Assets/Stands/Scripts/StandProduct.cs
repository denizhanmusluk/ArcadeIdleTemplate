using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandProduct : Stand
{


    public Transform[] productPosTR;
    [SerializeField] int customerCount;
    public CollectType collectTypeMachine;
    public Collectable[] productsPrefab;
    public GameObject tutorialPosTR;
    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
    }

    public override void CollectableCountSet()
    {
        PlayerPrefs.SetInt(machineName + "col" + PlayerPrefs.GetInt("level"), droppedCollectionList.Count);
        collectProduct.ratio = (float)droppedCollectionList.Count / (float)rawCountTotal;
    }
    public override void SpecificStart()
    {
        collectProduct.collectables = droppedCollectionList;
        MarketCustomerManager.Instance.collectAreaList.Add(collectProduct);
        MarketCustomerManager.Instance.maxCustomerCount += customerCount;
        foreach (var wrkArea in workAreaList)
        {
            wrkArea.standList.Add(this);
        }
        StartCoroutine(StartDelay());
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<Collider>().enabled = true;
        //StandReActive();
    }
 
    public override void SpecificReset()
    {

    }





    public override void DropCollection(int collectAmount, StackCollect _stackCollect)
    {
        if (collectAmount > 0 && rawCountCurrent > 0 && _stackCollect.collectionTrs[0] != null)
        {
            _stackCollect.collectActive = false;
            StartCoroutine(DropSequantial(collectAmount, _stackCollect));
        }
    }

    IEnumerator DropSequantial(int collectAmount, StackCollect _stackCollect)
    {
        Collectable droppingCollection = null;

        for (int i = 0; i < _stackCollect.collectionTrs.Count; i++)
        {
            foreach (int cltId in collectIDList)
            {
                if (_stackCollect.collectionTrs[i].collectID == cltId)
                {
                    if (collectType2 == CollectType.All || collectType == CollectType.All || _stackCollect.collectionTrs[i].collectType == collectType || _stackCollect.collectionTrs[i].collectType == collectType2)
                    {
                        droppingCollection = _stackCollect.collectionTrs[i];
                        i = _stackCollect.collectionTrs.Count;
                        break;
                    }

                }
            }
        }
        Collectable deletedCollect = droppingCollection;

        _stackCollect.collectionTrs.Remove(deletedCollect);


        droppedCollectionList.Add(droppingCollection);
        PlayerPrefs.SetInt(machineName + "col" + PlayerPrefs.GetInt("level"), droppedCollectionList.Count);

        yield return null;
        //droppingCollection.collectActive = false;
        float deltaY = 0;
        deltaY = (droppedCollectionList.Count - 1) / productPosTR.Length;
        Transform targetTR = productPosTR[(droppedCollectionList.Count - 1) % productPosTR.Length];
        Vector3 dropPos = targetTR.position + new Vector3(0, deltaY * 1.25f, 0);
        StartCoroutine(Drop(targetTR, dropPos, droppingCollection, Time.deltaTime));

        _stackCollect.collectActive = true;
    }

    IEnumerator Drop(Transform dropPosTR, Vector3 dropPos, Collectable collectable, float waitTime)
    {
        collectable.collectedList = droppedCollectionList;
        collectable.isCollected = false;

        yield return new WaitForSeconds(waitTime);
        Vector3 firstPos = collectable.transform.position;
        Quaternion firstRot = collectable.transform.rotation;
        if (collectable.gameObject != null)
        {
            firstPos = collectable.transform.position;
            firstRot = collectable.transform.rotation;
        }

        float timeCounter = 0;

        float angle = 0f;
        float posY = 0f;
        float posY_Factor = 5f;
        timeCounter = 0f;


        Quaternion targetAngle = dropPosTR.rotation;
        while (timeCounter < 1f)
        {
            timeCounter += 4 * Time.deltaTime;
            angle = timeCounter * Mathf.PI;
            posY = posY_Factor * Mathf.Sin(angle);


            if (collectable.gameObject != null)
            {
                collectable.transform.position = Vector3.Lerp(firstPos, new Vector3(dropPos.x, dropPos.y + posY, dropPos.z), timeCounter);
                collectable.transform.rotation = Quaternion.Lerp(firstRot, targetAngle, timeCounter);
            }
            yield return null;
        }
        collectable.transform.position = dropPos;
        collectable.productCollectActive = true;

        collectProduct.ratio = (float)droppedCollectionList.Count / (float)rawCountTotal;
    }
}

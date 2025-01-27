//using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorkArea : MonoBehaviour
{
    public List<Stand> standList;
    public List<CollectProduct> collectProductList;

    private System.Random rng = new System.Random();
    void Start()
    {

    }
    public void TargetArrived(AIWorker aiWorker)
    {
        StartCoroutine(Staying(aiWorker));
    }
    IEnumerator Staying(AIWorker character)
    {
        yield return new WaitForSeconds(Random.Range(2.5f, 3f));
        //ShuffleStandList();

        if (character.followActive)
        {
            character.TargetStandSelect();
        }
    }



    public void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public void ShuffleStandList()
    {
        List<Stand> sortedList = standList.OrderBy(x => (x.collectProduct.collectables.Count)).ToList();
        standList = sortedList;

        List<CollectProduct> sortedCollectList = collectProductList.OrderByDescending(x => (x.collectables.Count)).ToList();
        collectProductList = sortedCollectList;
        //Shuffle(CollectProductList);

        int index = 0;
        for(int i = 0; i < standList.Count; i++)
        {
            bool breakActive = false;

            foreach (int cltId in standList[i].collectIDList)
            {


                for (int j = 0; j < collectProductList.Count;j++)
                {
                    if (cltId == collectProductList[j].CollectId)
                    {
                        index = j;
                        breakActive = true;
                        break;
                    }
                }
                if (breakActive)
                {
                    break;
                }
            }
            if (breakActive)
            {
                break;
            }
        }
        //Shuffle2(CollectProductList, index);
        //List<CollectProduct> sortedList2 = CollectProductList.OrderByDescending(x => (x.collectables.Count)).ToList();
        //CollectProductList = sortedList2;
    }
    public void ShuffleCollectProductList()
    {
        Shuffle(collectProductList);
        List<CollectProduct> sortedList = collectProductList.OrderByDescending(x => (x.collectables.Count)).ToList();
        collectProductList = sortedList;
    }
    public void Shuffle2<T>(List<T> list, int index)
    {
        T value = list[0];
        list[0] = list[index];
        list[index] = value;
    }
}
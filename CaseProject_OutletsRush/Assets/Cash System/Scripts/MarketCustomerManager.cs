using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MarketCustomerManager : MonoBehaviour
{
    private static MarketCustomerManager _instance = null;
    public static MarketCustomerManager Instance => _instance;

    public GameObject[] characterPrefab;
    [SerializeField] float creatingPeriod;

    [HideInInspector] public List<CollectProduct> collectAreaList;
    [HideInInspector] public int maxCustomerCount = 0;

    private List<AIMarketCustomer> customerList = new List<AIMarketCustomer>();
    private System.Random rng = new System.Random();

    private void Awake()
    {
        _instance = this;
        maxCustomerCount = 0;
    }
    void Start()
    {
        StartCoroutine(CreatorElectronicsCustomer());
    }
    IEnumerator CreatorElectronicsCustomer()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            while (customerList.Count >= maxCustomerCount)
            {
                yield return null;
            }
            float offSetTime = Random.Range(0f, 1f);
            yield return new WaitForSeconds(creatingPeriod + offSetTime);
            Creating(MarketType.Electronics);
        }
    }


    void Creating(MarketType marketType)
    {
        int chaarcterSelect = Random.Range(0, characterPrefab.Length);
        AIMarketCustomer character;


        switch (marketType)
        {
            case MarketType.Electronics:
                {
                    character = SpawnManager.Instance.SpawnCustomer(characterPrefab[chaarcterSelect].transform, CheckoutManager.Instance.customerCreateTR.position, Quaternion.identity);

                        character.marketType = marketType;

                        CustomerAddList(character, marketType);
                        SortedCollectElectronicList(collectAreaList);
                        for (int i = 0; i < collectAreaList.Count; i++)
                        {
                            character.collectAreaList.Add(collectAreaList[i]);
                        }
                }
                break;

            case MarketType.Sports:
                {

                }
                break;

            case MarketType.Toys:
                {

                }
                break;
        }
    }
    public void CustomerAddList(AIMarketCustomer character, MarketType marketType)
    {
        switch (marketType)
        {
            case MarketType.Electronics:
                {
                    customerList.Add(character);
                }
                break;

            case MarketType.Sports:
                {
                }
                break;

            case MarketType.Toys:
                {
                }
                break;
        }
    }
    public void CustomerRemoveList(AIMarketCustomer character, MarketType marketType)
    {
        switch (marketType)
        {
            case MarketType.Electronics:
                {
                    customerList.Remove(character);
                }
                break;

            case MarketType.Sports:
                {
                }
                break;

            case MarketType.Toys:
                {
                }
                break;
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
    private void SortedCollectElectronicList(List<CollectProduct> CollectProductList)
    {
        List<CollectProduct> sortedList = CollectProductList.OrderByDescending(x => (x.ratio)).ToList();
        collectAreaList = sortedList;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckoutManager : Singleton<CheckoutManager>, IMoneyArea
{
    public List<Transform> customerWaitPosListTR;
    public GameObject activatorGO;
    public Transform customerCreateTR;
    public string cash;

    public bool cashActive = false;
    public bool cashWorkerActive = false;

    [SerializeField] private MoneyArea moneyArea;
    [SerializeField] private Transform exit;
    [SerializeField] private GameObject customerWaitProgressGO;
    [SerializeField] private Image customerProgressFill;
    [SerializeField] private Casier _casier;
    [SerializeField] private GameObject activatorGreenGO;
    [SerializeField] private GameObject activatorWhiteGO;

    [HideInInspector] public List<Transform> customerList;

    private void Start()
    {
        StartCoroutine(MoneyCreate());
    }
    public void MoneySave()
    {
        PlayerPrefs.SetInt(cash + "banknotcount", 0);
    }
    public void PayCustomer()
    {
        if (cashWorkerActive)
        {
            _casier._animator.SetBool("pay", true);
        }
        StartCoroutine(PayWaiting());
    }
    IEnumerator PayWaiting()
    {
        customerWaitProgressGO.SetActive(true);
        float counter = 0f;
        float waitingTime = 0.4f;
        while(counter < waitingTime)
        {
            counter += Time.deltaTime;
            Mathf.Clamp(counter, 0, waitingTime);
            customerProgressFill.fillAmount = Mathf.Lerp(0, 1, counter / waitingTime);
            yield return null;
        }
        //customerList[0].smileyGO.SetActive(true);

        if(customerList[0].GetComponent<AIMarketCustomer>() != null)
        {
            DropMoney(customerList[0].GetComponent<AIMarketCustomer>().aiStackCollect.collectionTrs);
        }
   
        customerProgressFill.fillAmount = 0;
        customerWaitProgressGO.SetActive(false);
    }


    void AllCustomersMove()
    {
        PlayerPrefs.SetInt("focuscounter", PlayerPrefs.GetInt("focuscounter") + 1);
        foreach (var cstmr in customerList)
        {
            cstmr.GetComponent<ICustomer>().CaseTargetPosSelect();
        }
    }

 

  
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<StackCollectPlayer>() != null)
        {
            cashActive = true;
            activatorGreenGO.SetActive(true);
            activatorWhiteGO.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<StackCollectPlayer>() != null)
        {
            cashActive = false;
            activatorGreenGO.SetActive(false);
            activatorWhiteGO.SetActive(true);
        }
    }
  
    IEnumerator MoneyCreate()
    {
        int moneyHeightBoundCount = 200;
        int banknotVal = 1;

        for (int i = 0; i < PlayerPrefs.GetInt(cash + "banknotcount"); i++)
        {
            float deltaY = 0;
            deltaY = (i % moneyHeightBoundCount) / (moneyArea.dropMoneyPosList.Count % moneyHeightBoundCount);
            Transform targetTR = moneyArea.dropMoneyPosList[(i) % moneyArea.dropMoneyPosList.Count];
            Vector3 dropPos = targetTR.position + new Vector3(0, deltaY * 0.2f, 0);
            BanknotMoney banknot = SpawnManager.Instance.SpawnMoney(moneyArea.moneyPrefab.transform, moneyArea.firstMoneyCreatePosTR.position, Quaternion.identity);
            banknot.MovingMoney(moneyArea.firstMoneyCreatePosTR.position, dropPos, targetTR);
            banknot.banknotValue = banknotVal;
            moneyArea.moneyList.Add(banknot);
            yield return null;
        }
    }

    void DropMoney(List<Collectable> droppingCollectionList)
    {
        StartCoroutine(DroppingMoney(droppingCollectionList));
    }
    IEnumerator DroppingMoney(List<Collectable> droppingCollectionList)
    {
        moneyArea.moneyCollectActive = false;
        int moneyHeightBoundCount = 200;
        int stepNo = 0;
        int moneyListCount = moneyArea.moneyList.Count;
        int totalMoney = 0;
        for (int i = 0; i < droppingCollectionList.Count; i++)
        {
            totalMoney += droppingCollectionList[i].price;
        }

        for (int i = 0; i < totalMoney; i++)
        {
            float deltaY = ((moneyListCount + i) % moneyHeightBoundCount) / (moneyArea.dropMoneyPosList.Count % moneyHeightBoundCount);

            Transform targetTR = moneyArea.dropMoneyPosList[(moneyListCount + i) % moneyArea.dropMoneyPosList.Count];
            Vector3 dropPos = targetTR.position + new Vector3(0, deltaY * 0.2f, 0);
            BanknotMoney banknot = SpawnManager.Instance.SpawnMoney(moneyArea.moneyPrefab.transform, moneyArea.firstMoneyCreatePosTR.position, Quaternion.identity);
            banknot.MovingMoney(moneyArea.firstMoneyCreatePosTR.position, dropPos, targetTR);
            banknot.banknotValue = 1;
         
            moneyArea.moneyList.Add(banknot);

            stepNo++;
            if (stepNo % 20 == 0)
            {
                yield return null;
            }
        }
        PlayerPrefs.SetInt(cash + "banknotcount", moneyArea.moneyList.Count);

        AIBehaviour();
        AllCustomersMove();

        yield return new WaitForSeconds(0.5f);
        moneyArea.moneyCollectActive = true;
    }
    void AIBehaviour()
    {
        customerList[0].GetComponent<ICustomer>().CustomerGoExit(exit);
        customerList.Remove(customerList[0]);
    }
}

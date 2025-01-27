using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using EZ_Pooling;

public class AIMarketCustomer : MonoBehaviour, ICustomer
{
    public MarketType marketType;

    public AIMoving aiMoving;
    public StackCollect aiStackCollect;

    public NavMeshAgent navMeshAgent;
    public Animator animator;


    public List<CollectProduct> collectAreaList;
    public List<CollectProduct> collectAreaTarget;
    public CollectProduct currentTarget;

    public bool shoppingActive = true;
    public float currentCommentPoint { get; set; }


    private Transform caseTargetPos;
    private Transform exitTR;

    private bool payingActive = false;
    private bool paymentActive = true;
    private bool checkActive = true;

    private int selectAreaCountTotal = 1;
    private int targetCollectId = 0;
    private int selectAreaCountCurrent = 0;
    private int targetPointSelect = 0;
    public void OnEnable()
    {
        StartCoroutine(EnableDelay());
    }
    IEnumerator EnableDelay()
    {
        yield return new WaitForSeconds(1f);
        int standSelect = UnityEngine.Random.Range(0, collectAreaList.Count);
        CollectProduct selectedStand = collectAreaList[standSelect];
        collectAreaTarget.Add(selectedStand);
        selectedStand.customerCount++;
        collectAreaList.Remove(selectedStand);
        TargetStandAreaSelect(true, false);

    }
    public void NextTarget()
    {
        currentTarget.aiCollectTargetTR[targetPointSelect].GetComponent<AICollectTarget>().isSelected = false;

        StartCoroutine(SameTargetCheckReset());
        selectAreaCountCurrent++;
        TargetStandAreaSelect(false, false);
    }
 
    public void TargetStandAreaSelect(bool variableStack,bool sameStand)
    {
        if (selectAreaCountTotal > selectAreaCountCurrent)
        {
            currentTarget = collectAreaTarget[selectAreaCountCurrent];
            int stackCount = 1;

            stackCount = UnityEngine.Random.Range((int)currentTarget.customerMinMax.x, (int)currentTarget.customerMinMax.y);

            StackTextInit();
            targetCollectId = currentTarget.CollectId;
            aiStackCollect.GetComponent<StackCollectMarketCustomer>().targetId = targetCollectId;

            targetPointSelect = UnityEngine.Random.Range(0, currentTarget.aiCollectTargetTR.Length);

            for (int i = 0; i < currentTarget.aiCollectTargetTR.Length; i++)
            {
                AICollectTarget aiCollectTarget = currentTarget.aiCollectTargetTR[i].GetComponent<AICollectTarget>();
                if (!aiCollectTarget.isSelected)
                {
                    aiCollectTarget.isSelected = true;
                    targetPointSelect = i;
                    break;
                }
            }
            aiStackCollect.collectActive = false;
            aiMoving.GoTargetStart(currentTarget.aiCollectTargetTR[targetPointSelect], Customer_Staying);
        }
        else
        {
            shoppingActive = false;
            CaseWaitListAdd();
        }
    }
    public void StackTextInit()
    {

    }
    public void CaseWaitListAdd()
    {
        CheckoutManager.Instance.customerList.Add(transform);
        CaseTargetPosSelect();
    }
    public void CaseTargetPosSelect()
    {
        payingActive = true;
        int waitingPosCount = CheckoutManager.Instance.customerWaitPosListTR.Count;
        if (CheckoutManager.Instance.customerList.Count > waitingPosCount)
        {
            Transform lastPosTR = CheckoutManager.Instance.customerWaitPosListTR[waitingPosCount - 1];
            CheckoutManager.Instance.customerWaitPosListTR.Add(lastPosTR);
        }
        caseTargetPos = CheckoutManager.Instance.customerWaitPosListTR[CheckoutManager.Instance.customerList.IndexOf(transform)];
        currentTarget.selectStand = false;
        StartCoroutine(GoCaseDelay());
    }
    IEnumerator GoCaseDelay()
    {
        yield return new WaitForSeconds(0.25f);
        aiMoving.GoTargetStart(caseTargetPos, WhenArrivedCashPos);

    }
    void WhenArrivedCashPos()
    {
        StartCoroutine(PosRotate(caseTargetPos.rotation));
        if (CheckoutManager.Instance.customerList.IndexOf(transform) == 0)
        {
            StartCoroutine(PaymentElectronics());
        }
    }

    IEnumerator PaymentElectronics()
    {
        while (paymentActive)
        {
            if (CheckoutManager.Instance.cashActive || CheckoutManager.Instance.cashWorkerActive)
            {
                paymentActive = false;
            }
            yield return null;
        }
        CheckoutManager.Instance.PayCustomer();
        yield return new WaitForSeconds(4f);
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }


    IEnumerator PosRotate(Quaternion targetRot)
    {
        Quaternion firstRot = transform.rotation;

        float counter = 0f;
        while (counter < 1f)
        {
            counter += 2 * Time.deltaTime;

            transform.rotation = Quaternion.Lerp(firstRot, targetRot, counter);
            yield return null;
        }
        transform.rotation = targetRot;
    }

    public void CustomerGoExit(Transform exitPosTR)
    {
        MarketCustomerManager.Instance.CustomerRemoveList(this, marketType);
        exitTR = exitPosTR;
        aiMoving.GoTargetStart(exitTR, DeSpawning); 
    }

    void DeSpawning()
    {
        foreach (var clt in aiStackCollect.collectionTrs)
        {
            clt.DeSpawn();
        }
        payingActive = false;
        paymentActive = true;
        shoppingActive = true;
        selectAreaCountCurrent = 0;
        currentTarget = null;
        collectAreaList.Clear();
        collectAreaTarget.Clear();
        aiStackCollect.collectionTrs.Clear();
        EZ_PoolManager.Despawn(this.transform);
    }
    public void Customer_Staying()
    {
        aiStackCollect.collectActive = true;
        int[] collectIDList = currentTarget._stand.collectIDList;
        StartCoroutine(CustomerStaying(collectIDList));
    }
    IEnumerator CustomerStaying(int[] collectIDList)
    {
        int selectAreaCountPre = selectAreaCountCurrent;
        bool newTargetSelected = false;
        yield return new WaitForSeconds(2f);
        bool goOut = false;
        while (!payingActive && !goOut && selectAreaCountPre == selectAreaCountCurrent && checkActive)
        {
            goOut = true;
            foreach (var clListId in collectIDList)
            {
                if (!payingActive && aiStackCollect.GetComponent<StackCollectMarketCustomer>().targetId == clListId)
                {

                    goOut = false;
                    if (!payingActive && NewSameTargetStand())
                    {

                        newTargetSelected = true;
                        goOut = true;
                    }
                    break;
                }
            }
            if (newTargetSelected)
            {
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        if (!goOut || !newTargetSelected)
        {
            if (currentTarget != null)
            {
                Customer_Staying();
            }
        }
    }


    public bool NewSameTargetStand()
    {

        List<CollectProduct> collectAreaOtherList = MarketCustomerManager.Instance.collectAreaList;   

        bool targetChanged = false;
        foreach (var _targetStand in collectAreaOtherList)
        {
            bool breakActive = false;

            if (!payingActive && checkActive && aiStackCollect.GetComponent<StackCollectMarketCustomer>().targetId == _targetStand.CollectId && _targetStand.collectables.Count > 0 && !_targetStand.selectStand)
            {
                _targetStand.selectStand = true;
                currentTarget.selectStand = false;
                currentTarget.aiCollectTargetTR[targetPointSelect].GetComponent<AICollectTarget>().isSelected = false;

                targetChanged = true;
                collectAreaTarget[selectAreaCountCurrent] = _targetStand;
                TargetStandAreaSelect(true, true);
                breakActive = true;
                break;
            }

            if (breakActive)
            {
                break;
            }
        }

        return targetChanged;
    }

    IEnumerator SameTargetCheckReset()
    {
        checkActive = false;
        yield return new WaitForSeconds(6f);
        checkActive = true;
    }  
}

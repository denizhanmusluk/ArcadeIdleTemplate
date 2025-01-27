using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWorker : MonoBehaviour
{
    public AIMoving aiMoving;
    public StackCollect aiStackCollect;
    public Animator animator;


    public bool followActive = false;
    public bool aiDropActive = true;
    public bool aiStackActive = false;

    [SerializeField] WorkArea workArea;
    [SerializeField] Stand targetStand;
    [SerializeField] CollectProduct collectAreaTarget;

    private int targetCollectPointSelect = 0;
    private int targetDropPointSelect = 0;
    private int targetCollectId = 0;
    Vector3 firstPos;

    private void Awake()
    {
        firstPos = transform.position;

    }
    public void DropActivator()
    {
        StartCoroutine(DropActivate());
    }
    IEnumerator DropActivate()
    {
        aiDropActive = false;
        yield return new WaitForSeconds(Time.deltaTime * 2);
        aiDropActive = true;
    }
    void Start()
    {
        //DnzEvents.aiWorkerStart += AIStart;
        //DnzEvents.aiWorkerStop += AIStop;
        //DnzEvents.aiWorkerListReset += ListReset;

        aiStackCollect.GetComponent<StackCollectWorker>().characterUpgradeSettings = UpgradeManager.Instance._upgradeSettings;

        MoveSpeedInit();

        StartCoroutine(StartFirst());
    }
    IEnumerator StartFirst()
    {
        yield return new WaitForSeconds(1f);
        AIStart();
        //aiMoving.GoTargetStart(firstPos, AIStart);
    }
    public void MoveSpeedInit()
    {
        aiMoving.MoveSpeedInit(aiStackCollect.GetComponent<StackCollectWorker>().characterUpgradeSettings.workerSpeed[Globals.workerMoveSpeedLevel]);
    }
    public void ListReset()
    {
        aiStackCollect.GetComponent<StackCollectWorker>().collectActive = true;
        foreach (var collection in aiStackCollect.collectionTrs)
        {
            Destroy(collection.gameObject);
        }
        aiStackCollect.collectionTrs.Clear();
    }

    void AIStart()
    {
        followActive = true;
        StartCoroutine(AIStartDelay());
    }
    public void AIStop()
    {
        aiStackCollect.GetComponent<StackCollectWorker>().collectActive = false;
        followActive = false;
        aiMoving.GoTargetStart(firstPos, null);

    }
    IEnumerator AIStartDelay()
    {
        yield return new WaitForSeconds(1f);
        TargetStandAreaSelect();

    }
    IEnumerator AiStackCountInit(int i)
    {
        yield return new WaitForSeconds(2f);
        aiStackCollect.GetComponent<StackCollectWorker>().currentCapacity = aiStackCollect.GetComponent<StackCollectWorker>().collectionTrs.Count + 10;
    }
    public void TargetStandAreaSelect()
    {
        workArea.ShuffleStandList();
        List<CollectProduct> tempCollectArea = new List<CollectProduct>();
        bool collectTargetCheckActive = true;
        foreach (var env in workArea.collectProductList)
        {
            if (env.collectables.Count > 0 && collectTargetCheckActive && env.collectActive)
            {
                for (int i = 0; i < workArea.standList.Count; i++)
                {
                    foreach (int cltId in workArea.standList[workArea.standList.Count - 1 - i].collectIDList)
                    {
                        bool breakActive = false;
                        if ((workArea.standList[workArea.standList.Count - 1 - i].collectType == CollectType.All || env.collectables[env.collectables.Count - 1].collectType == workArea.standList[workArea.standList.Count - 1 - i].collectType || workArea.standList[workArea.standList.Count - 1 - i].collectType2 == CollectType.All || env.collectables[env.collectables.Count - 1].collectType == workArea.standList[workArea.standList.Count - 1 - i].collectType2) && cltId == env.CollectId && workArea.standList[workArea.standList.Count - 1 - i].rawCountCurrent > 0)
                        {
                            StartCoroutine(AiStackCountInit(i));
                            tempCollectArea.Add(env);
                            targetStand = workArea.standList[workArea.standList.Count - 1 - i];

                            collectTargetCheckActive = false;
                            breakActive = true;
                            break;
                        }
                        if (breakActive)
                        {
                            break;
                        }
                    }
                }
            }
        }
        if (tempCollectArea.Count != 0)
        {
            collectAreaTarget = tempCollectArea[0];
            targetCollectId = collectAreaTarget.CollectId;
            aiStackCollect.GetComponent<StackCollectWorker>().targetId = targetCollectId;
            aiStackCollect.GetComponent<StackCollectWorker>().collectableType = collectAreaTarget.collectableType;

            collectAreaTarget.aiCollectTargetCheck[targetCollectPointSelect] = 0;

            targetCollectPointSelect = UnityEngine.Random.Range(0, collectAreaTarget.aiCollectTargetTR.Length);
            for (int i = 0; i < 50; i++)
            {
                targetCollectPointSelect = UnityEngine.Random.Range(0, collectAreaTarget.aiCollectTargetTR.Length);
                if (collectAreaTarget.aiCollectTargetCheck[targetCollectPointSelect] != 1)
                {
                    collectAreaTarget.aiCollectTargetCheck[targetCollectPointSelect] = 1;
                    break;
                }
            }
            aiStackCollect.GetComponent<StackCollectWorker>().collectActive = false;
            //aiStackActive = false;
            aiMoving.GoTargetStart(collectAreaTarget.aiCollectTargetTR[targetCollectPointSelect], WorkAreaTargetArrived);
        }
        else
        {
            aiMoving.GoTargetStart(firstPos, AIStart);
        }
    }
    void WorkAreaTargetArrived()
    {
        aiStackActive = true;
        aiStackCollect.GetComponent<StackCollectWorker>().collectActive = true;
        workArea.TargetArrived(this);
    }
    public bool NewSameTargetStand()
    {
        bool targetChanged = false;
        for (int i = 0; i < workArea.standList.Count; i++)
        {
            bool breakActive = false;
            foreach (Collectable clt in aiStackCollect.collectionTrs)
            {
                foreach (int cltId2 in workArea.standList[workArea.standList.Count - 1 - i].collectIDList)
                {
                    if ((workArea.standList[workArea.standList.Count - 1 - i].collectType == CollectType.All || clt.collectType == workArea.standList[workArea.standList.Count - 1 - i].collectType || workArea.standList[workArea.standList.Count - 1 - i].collectType2 == CollectType.All || clt.collectType == workArea.standList[workArea.standList.Count - 1 - i].collectType2) && clt.collectID == cltId2 && workArea.standList[workArea.standList.Count - 1 - i].rawCountCurrent > 0)
                    {
                        targetChanged = true;
                        targetStand = workArea.standList[workArea.standList.Count - 1 - i];
                        TargetStandSelect();
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

        return targetChanged;
    }
    public void TargetStandSelect()
    {
        aiStackCollect.GetComponent<StackCollectWorker>().aiCollectActive = false;
        targetStand.aiTargetPosListCheck[targetDropPointSelect] = 0;

        targetDropPointSelect = Random.Range(0, targetStand.aiTargetPosList.Length);
        for (int i = 0; i < 50; i++)
        {
            targetDropPointSelect = Random.Range(0, targetStand.aiTargetPosList.Length);
            if (targetStand.aiTargetPosListCheck[targetDropPointSelect] != 1)
            {
                targetStand.aiTargetPosListCheck[targetDropPointSelect] = 1;
                break;
            }
        }
        aiStackCollect.GetComponent<StackCollectWorker>().collectActive = false;
        aiDropActive = false;

        aiMoving.GoTargetStart(targetStand.aiTargetPosList[targetDropPointSelect], StandArrived);
    }
    void StandArrived()
    {
        aiStackActive = true;
        aiDropActive = true;
        targetStand.TargetArrived(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectProduct : MonoBehaviour
{
    public enum WorkType { player_worker, customer, sushiCustomer }
    public WorkType Type;

    public Vector2 customerMinMax;
    public int CollectId;
    public List<Collectable> collectables;
    public Transform[] aiCollectTargetTR;
    public int[] aiCollectTargetCheck;
    public Stand _stand;
    public Sprite standSprite;
    public int customerCount = 0;
    public WorkArea _FishDropArea;
    public CollectType collectableType;
    public bool collectActive = true;

    public GameObject noneCollectGO;

    public float ratio;
    public Transform boxPosTR;

    public List<Transform> bandPosList;

    public bool selectStand = false;
    private void Awake()
    {

    }
    private void Start()
    {
        aiCollectTargetCheck = new int[aiCollectTargetTR.Length];
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() || other.GetComponent<StackCollectWorker>())
        {
            if (_stand != null)
            {
                _stand.productGoActive = true;
            }
            if (noneCollectGO != null)
            {
                noneCollectGO.SetActive(false);
            }
        }
    }
    IEnumerator ColliderReset()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(Time.deltaTime * 2);
        GetComponent<Collider>().enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        switch (Type)
        {
            case WorkType.player_worker:
                {
                    if (other.GetComponent<StackCollectPlayer>() != null && collectables.Count > 0)
                    {

                        if (collectables[collectables.Count - 1].productCollectActive
                            && other.GetComponent<StackCollectPlayer>().collectionTrs.Count < (other.GetComponent<StackCollectPlayer>()._playerController._characterUpgradeSettings.stackCapacity[Globals.stackCapacityLevel]))
                        {

                            collectables[collectables.Count - 1].productCollectActive = false;

                            other.GetComponent<StackCollectPlayer>().Collecting(collectables[collectables.Count - 1], 0f);
                            StartCoroutine(ColliderReset());
                            if (_stand != null)
                            {
                                _stand.collectableCount--;
                                _stand.CollectableCountSet();
                            }
                        }
       
                    }

                    if (other.GetComponent<StackCollectWorker>() != null && collectables.Count > 0 && CollectId == other.GetComponent<StackCollectWorker>().targetId && collectableType == other.GetComponent<StackCollectWorker>().collectableType && other.GetComponent<StackCollectWorker>().collectActive)
                    {
                        if (collectables[collectables.Count - 1].productCollectActive && other.GetComponent<StackCollectWorker>().collectionTrs.Count < (other.GetComponent<StackCollectWorker>().baseStackCapacity  + other.GetComponent<StackCollectWorker>().characterUpgradeSettings.workerStackCapacity[Globals.workerStackLevel] ))
                        {
                            if (other.GetComponent<StackCollectWorker>().collectionTrs.Count < other.GetComponent<StackCollectWorker>().currentCapacity)
                            {
                                collectables[collectables.Count - 1].productCollectActive = false;
                                other.GetComponent<StackCollectWorker>().Collecting(collectables[collectables.Count - 1] , 0f);
                                StartCoroutine(ColliderReset());
                                if (_stand != null)
                                {
                                    _stand.collectableCount--;
                                    _stand.CollectableCountSet();
                                }

                            }
                        }
                    }
                }
                break;
            case WorkType.customer:
                {       
                    if (other.GetComponent<StackCollectMarketCustomer>() != null && CollectId == other.GetComponent<StackCollectMarketCustomer>().targetId && other.GetComponent<StackCollectMarketCustomer>()._aiCustomer.currentTarget.collectableType == collectableType && other.GetComponent<StackCollectMarketCustomer>().collectActive)
                    {
                        if (other.GetComponent<StackCollectMarketCustomer>()._aiCustomer.shoppingActive)
                        {
                            if (other.GetComponent<StackCollectMarketCustomer>().collectionTrs.Count < other.GetComponent<StackCollectMarketCustomer>().stackCapacity)
                            {
                                if (collectables.Count > 0 && collectables[collectables.Count - 1].productCollectActive && collectActive)
                                {
                                    collectables[collectables.Count - 1].productCollectActive = false;

                                    other.GetComponent<StackCollectMarketCustomer>().Collecting(collectables[collectables.Count - 1] , 0.5f);

                                    _stand.rawCountCurrent++;
                                    other.GetComponent<StackCollectMarketCustomer>()._aiCustomer.StackTextInit();
                                    StartCoroutine(ColliderReset());
                                    if (_stand != null)
                                    {
                                        _stand.CollectableCountSet();
                                    }
                                }

                            }
                            else
                            {
                                other.GetComponent<StackCollectMarketCustomer>()._aiCustomer.NextTarget();
                                customerCount--;
                            }
                        }
                    }

                }
                break;
        }
    }


    bool fullTextActive = true;
    IEnumerator FullCapacity()
    {

        if (fullTextActive)
        {
            //GameManager.Instance.ui.fullCapacityText.SetActive(true);
            fullTextActive = false;
            float counter = 0f;
            while (counter < 2f)
            {
                counter += Time.deltaTime;
                yield return null;
            }
            fullTextActive = true;
            //GameManager.Instance.ui.fullCapacityText.SetActive(false);
        }
        yield return null;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum MarketType
{
    Electronics,
    Sports,
    Toys,
    Null
}
public abstract class Stand : MonoBehaviour
{
    public string machineName;
    public MarketType marketType;

    public CollectType collectType;
    public CollectType collectType2;

    public CollectProduct collectProduct;


    public int collectableCount = 0;

    public Transform[] aiTargetPosList;
    public int[] aiTargetPosListCheck;

    public TextMeshProUGUI rawCountText;
    public int rawCountTotal;
    public int rawCountCurrent;
    public int productCountTotal;

    public int[] collectIDList;
    public bool dropActive = true;
    public bool dropActiveAI = true;
    bool woodSetActive;
    public bool StandActive = false;
    public bool playerTriggerActive = true;
    public List<Collectable> droppedCollectionList = new List<Collectable>();
    public bool targetActive = true;
    public List<WorkArea> workAreaList;
    public bool productGoActive = true;

    public bool resetActive = false;
    public float waitTime = 1f;
    public bool customerCar = false;
    public abstract void SpecificStart();
    public abstract void SpecificReset();
    public abstract void DropCollection(int collectAmount, StackCollect _stackCollect);
    public abstract void CollectableCountSet();


    [SerializeField] bool onetoOneDropActive;
    public bool machineActive = false;
    private void Start()
    {
        aiTargetPosListCheck = new int[aiTargetPosList.Length];

        SpecificStart();

        TextInit();
    }

    public void TextInit()
    {
        rawCountCurrent = rawCountTotal;
        if (rawCountText != null)
        {
            rawCountText.text = (0).ToString() + "/" + (rawCountTotal).ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<StackCollectPlayer>() != null && StandActive && playerTriggerActive)
        {
            PlayerController _playerController = other.GetComponent<StackCollectPlayer>()._playerController;
            if (_playerController.dropActive)
            {
                _playerController.DropActivator();
                CollectionChecking(_playerController._stackCollect);
                StartCoroutine(ColliderReset());
            }
        }
        if (other.GetComponent<StackCollectWorker>() != null && StandActive)
        {
            StackCollectWorker _stackCollectWorker = other.GetComponent<StackCollectWorker>();
            if (_stackCollectWorker._aiWorker.aiDropActive && _stackCollectWorker._aiWorker.aiStackActive)
            {
                _stackCollectWorker._aiWorker.DropActivator();
                CollectionChecking(_stackCollectWorker._aiWorker.aiStackCollect);
                StartCoroutine(ColliderReset());
            }
        }

    }
    IEnumerator ColliderReset()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(Time.deltaTime * 1);
        GetComponent<Collider>().enabled = true;
    }

    void CollectionChecking(StackCollect _stackCollect)
    {

        int collectedNo = 0;
        foreach (var collects in _stackCollect.collectionTrs)
        {
            foreach (int cltId in collectIDList)
            {
                if (collects.collectID == cltId)
                {

                    if (customerCar)
                    {
                        if (rawCountCurrent > 0)
                        {
                            collectedNo++;
                            break;
                        }
                    }
                    else
                    {
                        if (collectType2 == CollectType.All || collectType == CollectType.All || collects.collectType == collectType || collects.collectType == collectType2)
                        {
                            collectedNo++;
                            break;
                        }
                    }
                }
            }
          
        }

        _stackCollect.ReverseCollectedList();
        _stackCollect.CollectedListReset();

        if (customerCar && collectedNo > 0)
        {
            DropCollection(rawCountCurrent, _stackCollect);
        }
        else
        {
            if (collectedNo > 0 && rawCountCurrent > 0)
            {
                if (collectedNo <= rawCountCurrent)
                {
                    DropCollection(collectedNo, _stackCollect);
                    StartCoroutine(SetFishAmount(-collectedNo));
                }
                else
                {
                    DropCollection(rawCountCurrent, _stackCollect);
                    StartCoroutine(SetFishAmount(-rawCountCurrent));
                }
            }
        }
    }
    IEnumerator SetFishAmount(int amount)
    {
        int Old = rawCountCurrent;
        if (onetoOneDropActive)
        {
            rawCountCurrent -= 1;
        }
        else
        {
            rawCountCurrent += amount;
        }

        woodSetActive = false;
        yield return null;
        yield return null;
        woodSetActive = true;
        float counter = 0f;
        while (counter < 1f && woodSetActive)
        {
            counter += Time.deltaTime;
            float value = Mathf.Lerp((float)Old, (float)rawCountCurrent, counter);

            if (rawCountText != null)
            {
                rawCountText.text = (rawCountTotal - (int)value).ToString() + "/" + (rawCountTotal).ToString();
            }

            yield return null;
        }
        if (rawCountText != null)
        {
            rawCountText.text = (rawCountTotal - rawCountCurrent).ToString() + "/" + (rawCountTotal).ToString();
        }
        if (rawCountCurrent == 0 && !resetActive)
        {
            resetActive = true;
            SpecificReset();
        }
    }


    public void TargetArrived(AIWorker aiWorker)
    {
        StartCoroutine(Staying(aiWorker));
    }
    IEnumerator Staying(AIWorker character)
    {
        bool newTargetSelected = false;
        yield return new WaitForSeconds(2f);
        bool goOut = false;
        while (!goOut)
        {
            goOut = true;
            foreach (var sk in character.aiStackCollect.collectionTrs)
            {
                goOut = true;
                foreach (var clListId in collectIDList)
                {
                    if (sk.collectID == clListId)
                    {
                        goOut = false;
                        if (character.NewSameTargetStand())
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
            }
            yield return null;
        }
        if (character.followActive && !newTargetSelected)
        {
            character.TargetStandAreaSelect();
        }
        yield return new WaitForSeconds(0.2f);
    }



    public void StandReActive()
    {
        StandActive = true;
        GetComponent<Collider>().enabled = true;
    }
}

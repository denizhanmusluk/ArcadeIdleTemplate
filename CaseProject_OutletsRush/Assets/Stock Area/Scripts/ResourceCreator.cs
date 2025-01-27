using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResourceCreator : Stand
{
    [SerializeField] Transform createTR;
    public Collectable[] productsPrefab;

    [SerializeField] Animator machineAnimator;
    public Transform[] collectProductPosTR;

    [SerializeField] List<GameObject> carPrefabList = new List<GameObject>();
    [SerializeField] Transform carCreateTR, carStandPos, carGoTR;
    [SerializeField] GameObject canvasProductGO, canvasDeliveringGO;

    [SerializeField] Image imageFill;
    [SerializeField] float cooldownTime;
    [SerializeField] int totalCapacity;
    int currentCount = 0;


    public List<Collectable> threadCollectionList = new List<Collectable>();
    bool creatingActive = false;
    float speedFactor = 1f;
    ResourceCar currentCar;
    public bool active = false;

    public int[] capacitiesProduct;
    public int[] capacitiesCar;
    public int[] cooldownTimes;

    public int standLevel { get; set; }

    public int _standUpgradeLevel = 0;
 
    void CapacityInit()
    {
        productCountTotal = capacitiesProduct[_standUpgradeLevel];
        totalCapacity = capacitiesCar[_standUpgradeLevel];
        cooldownTime = cooldownTimes[_standUpgradeLevel];

        rawCountText.text = (productCountTotal - rawCountCurrent).ToString() + "/" + (productCountTotal).ToString();
    }
    public override void CollectableCountSet()
    {
        collectableCount = threadCollectionList.Count;
        rawCountText.text = (collectableCount).ToString() + "/" + (productCountTotal).ToString();
    }


    public override void DropCollection(int collectAmount, StackCollect _stackCollect)
    {

    }

    public override void SpecificReset()
    {
        KepenkClose();
        rawCountText.text = (collectableCount).ToString() + "/" + (productCountTotal).ToString();
    }

    public override void SpecificStart()
    {
        collectProduct.collectables = threadCollectionList;

        foreach (var wrkArea in workAreaList)
        {
            wrkArea.standList.Add(this);
            wrkArea.collectProductList.Add(collectProduct);
        }

        StartCoroutine(CreatorChecking());

        KepenkOpen();
        CarCreate();
        CapacityInit(); 
    }
    IEnumerator CreatorChecking()
    {
        creatingActive = false;
        while (!creatingActive)
        {

            if (collectableCount < productCountTotal)
            {
                creatingActive = true;

                StartCoroutine(CannedCreator());
            }
            if (!creatingActive)
            {
                foreach (var lst in threadCollectionList)
                {
                    lst.productCollectActive = true;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator CannedCreator()
    {
        while (collectableCount < productCountTotal && active)
        {
            StartCoroutine(CreateCanned());
            dropActive = false;

            collectableCount++;
            currentCount++;
            if (currentCount % totalCapacity == 0)
            {
                StartCoroutine(ResetDelay());
            }
            rawCountText.text = (collectableCount).ToString() + "/" + (productCountTotal).ToString();

            yield return new WaitForSeconds(waitTime / speedFactor);
        }

        yield return new WaitForSeconds(1f);
        collectableCount = threadCollectionList.Count;

        StartCoroutine(CreatorChecking());
   
    }
    IEnumerator CreateCanned()
    {
        float deltaY = 0;
        Transform targetTR;

        foreach (var lst in threadCollectionList)
        {
            lst.productCollectActive = true;
        }
        int rawSelect = Random.Range(0, productsPrefab.Length);
        Collectable _newProduct = SpawnManager.Instance.SpawnColectables(productsPrefab[rawSelect].transform, createTR.position, createTR.rotation);
        //Collectable _newProduct = Instantiate(productsPrefab[rawSelect].gameObject, createTR.position, createTR.rotation).GetComponent<Collectable>();

        _newProduct.collectedList = threadCollectionList;
        threadCollectionList.Add(_newProduct);


        if (threadCollectionList.Count == 0)
        {
            deltaY = 0;
            targetTR = collectProductPosTR[0];
        }
        else
        {
            deltaY = (threadCollectionList.Count - 1) / collectProductPosTR.Length;
            targetTR = collectProductPosTR[(threadCollectionList.Count - 1) % collectProductPosTR.Length];
        }



        Vector3 dropPos = targetTR.position + new Vector3(0, deltaY * 1.25f, 0);

        StartCoroutine(GoCannedPos(_newProduct, targetTR, dropPos));
        yield return null;
    }
    IEnumerator GoCannedPos(Collectable newProduct, Transform targetTR, Vector3 dropPos)
    {
        newProduct.productCollectActive = true;
        Vector3 firstPos = newProduct.transform.position;
        Quaternion firstRot = newProduct.transform.rotation;
        Quaternion targetRot = targetTR.transform.rotation;

        float _speed = 2f;
        float angle = 0f;
        float posY = 0f;
        float psoY_Factor = 4f;
        float counter = 0f;
        while (counter < 1f && !newProduct.isCollected)
        {
            counter += _speed * Time.deltaTime;
            counter = Mathf.Clamp01(counter);


            angle = counter * Mathf.PI;
            posY = psoY_Factor * Mathf.Sin(angle);


            Vector3 targetPos = Vector3.Lerp(firstPos, dropPos, counter);
            targetPos.y += posY;


            newProduct.transform.position = targetPos;
            newProduct.transform.rotation = Quaternion.Lerp(firstRot, targetRot, counter);

            yield return null;
        }


        if (!newProduct.isCollected)
        {
            newProduct.transform.position = dropPos;
            newProduct.transform.rotation = targetRot;
        }
    }
    void KepenkClose()
    {
        if (machineAnimator != null)
        {
            machineAnimator.SetBool("close", true);
        }
        machineActive = true;
    }
    void KepenkOpen()
    {
        if (machineAnimator != null)
        {
            machineAnimator.SetBool("close", false);
        }
        machineActive = false;
    }
    void CarCreate()
    {
        currentCar = Instantiate(carPrefabList[0], carCreateTR.position, Quaternion.identity).GetComponent<ResourceCar>();
        currentCar.stand = this;
        currentCar.standPos = carStandPos;
        currentCar.carGoPos = carGoTR;
    }
    public void CarArrivedStockArea()
    {
        active = true;
        KepenkOpen();
    }
    IEnumerator ResetDelay()
    {
        active = false;
        StandActive = false;
        yield return new WaitForSeconds(0.11f);

        currentCar.GetComponent<ResourceCar>().CarGoOut();
        KepenkClose();

        canvasDeliveringGO.SetActive(true);
        canvasProductGO.SetActive(false);
        float counter = 0f;
        while (counter < cooldownTime)
        {
            counter += Time.deltaTime;
            imageFill.fillAmount = counter / cooldownTime;
            yield return null;
        }

        imageFill.fillAmount = 1;
        canvasDeliveringGO.SetActive(false);
        canvasProductGO.SetActive(true);

        TextInit();
        CarCreate();

        yield return new WaitForSeconds(1f);
        resetActive = false;
    }


}

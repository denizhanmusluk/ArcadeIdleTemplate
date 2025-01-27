using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

[System.Serializable]
public abstract class StackCollect : MonoBehaviour
{
    [Range(1.0f, 100.0f)]
    [SerializeField] float oscillationHalfLife = 10f;
    [SerializeField] float elasticRatio = 0.1f;
    [SerializeField] float turnSpeed = 100f;

    [SerializeField] public List<Collectable> collectionTrs = new List<Collectable>();
    [SerializeField] float stackStepOffset;
    int currentStackCount = 0;
    public bool collectActive = true;


    public Transform stackPos;

    public bool isPlayer;
    public FastIKFabric ikLeft;
    public FastIKFabric ikRight;

    public Transform leftIkTarget;
    public Transform rightIkTarget;

    public Transform leftProductTarget;
    public Transform rightProductTarget;

    public Transform leftBoxTarget;
    public Transform rightBoxTarget;

    public Transform leftNullTarget;
    public Transform rightNullTarget;
    public abstract void StackAnimation();
    public abstract void StackEmptyAnimation();

    public abstract float StackFollowSpeed();

    public void DisperseCollected(Transform impulseTR)
    {
        if (disperseActive)
        {
            List<Collectable> tempColList = new List<Collectable>();

            for (int i = 0; i < collectionTrs.Count; i++)
            {
                if (collectionTrs[i].collectID == 0)
                {
                    currentStackCount--;
                    collectionTrs[i].transform.parent = null;
                    tempColList.Add(collectionTrs[i]);
                }
            }

            for (int i = 0; i < tempColList.Count; i++)
            {
                collectionTrs.Remove(tempColList[i]);
            }

            if (collectionTrs.Count == 0)
            {
                StackEmptyAnimation();
            }
            StartCoroutine(DisperseActivator());
        }
    }
    bool disperseActive = true;
    IEnumerator DisperseActivator()
    {
        disperseActive = false;
        yield return new WaitForSeconds(1f);
        disperseActive = true;
    }
    public void Collecting(Collectable collectable, float collectDelay)
    {
        if (collectActive)
        {
            //collectable.collectActive = false;
            collectionTrs.Add(collectable);

            currentStackCount = collectionTrs.Count;
            StackAnimation();

            Transform targetTR = stackPos;
       
            float deltaHeight = (collectionTrs.Count - 1);

            StartCoroutine(SetToPos(collectable, targetTR, deltaHeight, collectDelay));

            collectable.collectedList.Remove(collectable);


        }
    }
    public void ReverseCollectedList()
    {
        List<Collectable> tempCollectionList = new List<Collectable>();

        for (int i = 0; i < collectionTrs.Count; i++)
        {
            tempCollectionList.Add(collectionTrs[i]);
        }

        for (int i = 0; i < collectionTrs.Count; i++)
        {
            collectionTrs[i] = tempCollectionList[collectionTrs.Count - i - 1];
        }
        currentStackCount = collectionTrs.Count;
        if (currentStackCount == 0)
        {
            StackEmptyAnimation();
        }

    }
    public void CollectedListReset()
    {
        ReverseCollectedList();
        StartCoroutine(ListPosReset(false));
    }
    public void CollectedListResetLevelUp()
    {
        ReverseCollectedList();
        StartCoroutine(ListPosReset(true));
    }
    IEnumerator ListPosReset(bool parentActive)
    {
        yield return new WaitForSeconds(0.1f);
        if (collectionTrs.Count == 0)
        {
            StackEmptyAnimation();
        }

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < collectionTrs.Count; i++)
        {
            Transform targetTR = stackPos;
            float deltaHeight = i;

            StartCoroutine(CollectablePosReset(collectionTrs[i], targetTR, deltaHeight, false));

            yield return null;
        }
    }
    IEnumerator CollectablePosReset(Collectable collectable, Transform targetTR, float deltaY, bool parentActive)
    {
        Vector3 dropPos = targetTR.position + new Vector3(0, deltaY * stackStepOffset, 0);

        Vector3 firstPos = collectable.transform.position;
        Quaternion firstRot = collectable.transform.rotation;
        float timeCounter = 0;

        while (timeCounter < 1f)
        {
            timeCounter += 4 * Time.deltaTime;
            firstPos = collectable.transform.position;
            firstRot = collectable.transform.rotation;
            dropPos = targetTR.position + new Vector3(0, deltaY * stackStepOffset, 0);
            collectable.transform.position = Vector3.Lerp(firstPos, dropPos, timeCounter);
            collectable.transform.rotation = Quaternion.Lerp(firstRot, transform.rotation, timeCounter);

            yield return null;
        }
        collectable.transform.position = dropPos;
        if (parentActive)
        {
            collectable.transform.parent = targetTR;
        }

    }

    private IEnumerator SetToPos(Collectable collectable, Transform targetTR, float deltaY, float collectDelay)
    {
        collectable.stackFollowingActive = false;

        collectable.isCollected = true;


        Vector3 dropPos = targetTR.position + new Vector3(0, deltaY * stackStepOffset, 0);

        Vector3 firstPos = collectable.transform.position;
        Quaternion firstRot = collectable.transform.rotation;

        float timeCounter = 0;
        Quaternion targetAngle = Quaternion.Euler(0, 0, 0);



        yield return new WaitForSeconds(collectDelay);


        float _speed = 2f;
        float angle = 0f;
        float posY = 0f;
        float posY_Factor = 4f;
        timeCounter = 0f;
        while (timeCounter < 1f)
        {
            timeCounter += _speed * Time.deltaTime;
            timeCounter = Mathf.Clamp01(timeCounter);

            angle = timeCounter * Mathf.PI;
            posY = posY_Factor * Mathf.Sin(angle);

            dropPos = targetTR.position + new Vector3(0, deltaY * stackStepOffset, 0);
            Vector3 targetPos = Vector3.Lerp(firstPos, dropPos, timeCounter);
            targetPos.y += posY;


            collectable.transform.position = targetPos;
            collectable.transform.localRotation = Quaternion.Lerp(firstRot, targetAngle, timeCounter);

            yield return null;
        }
        dropPos = targetTR.position + new Vector3(0, deltaY * stackStepOffset, 0);
        collectable.transform.position = dropPos; 
        collectable.transform.localRotation = targetAngle;
        collectable.stackFollowingActive = true;
        collectable.ScaleEffect();
    }

    void LateUpdate()
    {
        if (collectionTrs.Count > 0)
        {
            if (collectionTrs[0].stackFollowingActive)
            {
                FollowingStacks(stackPos, collectionTrs[0].transform, 0f);
            }
        }
        for (int i = 0; i < collectionTrs.Count - 1; i++)
        {
            if (collectionTrs[i].stackFollowingActive)
            {
                FollowingStacks(collectionTrs[i].transform, collectionTrs[i + 1].transform, i);
            }
        }
    }
    float counter = 0.5f;
    Vector3 distance = Vector3.zero;

    void FollowingStacks(Transform obj1, Transform obj2, float multiply)
    {
        float angleFactor = 0f;

        Vector3 targetPos = stackPos.position;

        if (PlayerController.Instance.pressActive && isPlayer)
        {
            targetPos = stackPos.position - transform.forward * elasticRatio * Mathf.Pow(multiply, 2);
        }

        float followSpeed = (1f * Vector3.Distance(obj2.position, obj1.position) + 1.5f) * StackFollowSpeed();



        float extraSpeed = 1f;
        Vector3 factor = Vector3.zero;
        distance = -(obj2.position - obj1.position).normalized;
        counter = 0.5f;

        Vector3 followPosition = new Vector3(targetPos.x, obj2.position.y, targetPos.z) + factor;

        obj2.position = Vector3.MoveTowards(obj2.position, followPosition, followSpeed * oscillationHalfLife * 0.1f * Time.deltaTime * extraSpeed);
        //obj2.position = Vector3.MoveTowards(obj2.position, followPosition, (followSpeed - (0.1f * multiply)) * oscillationHalfLife * 0.1f * Time.deltaTime * extraSpeed - multiply * elasticRatio);

        float deltaRotY = Quaternion.Angle(obj2.rotation, obj1.rotation);

        if (deltaRotY > 10)
        {
            deltaRotY *= 2;
        }
        Quaternion targetRot = obj1.rotation;
        obj2.rotation = Quaternion.RotateTowards(obj2.rotation, targetRot, Mathf.Abs(deltaRotY) * turnSpeed * Time.deltaTime);
    }

}

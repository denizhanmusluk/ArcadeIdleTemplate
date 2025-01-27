using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ_Pooling;
using DG.Tweening;

public class BanknotMoney : MonoBehaviour
{
    public int banknotValue;
    private Vector3 firstPos;
    private Vector3 targetPos;

    public void MovingMoney(Vector3 _firstPos, Vector3 _targetPos, Transform targetTR)
    {
        Vector3 randomPos = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
        firstPos = _firstPos + randomPos;
        targetPos = _targetPos;
        StartCoroutine(MoveDropMoney(targetTR));
    }
    IEnumerator MoveDropMoney(Transform targetTR)
    {
        Quaternion targetRot = Quaternion.Euler(0, targetTR.eulerAngles.y + Random.Range(-15, 15), 0);
        Quaternion firstRot = transform.rotation;
        float counter = 0f;
        float moveSpeed = Random.Range(10f, 20f);

        float angle = 0f;
        float posY = 0f;
        float psoY_Factor = 3f;
        while (counter < 1f)
        {
            counter += moveSpeed * Time.deltaTime;

            angle = counter * Mathf.PI;
            posY = psoY_Factor * Mathf.Sin(angle);

            transform.position = Vector3.Lerp(firstPos, new Vector3(targetPos.x, targetPos.y + posY, targetPos.z), counter);
            transform.rotation = Quaternion.Lerp(firstRot, targetRot, counter);
            yield return null;
        }
        transform.position = targetPos;

    }
    public void MoneyCollecting(Vector3 firstPos, Transform targetTr)
    {
        Vector3 randomRotSpeed = new Vector3(Random.Range(200f, 720f), Random.Range(200f, 720f), Random.Range(200f, 720f));
        float midPointHeight = Mathf.Max(firstPos.y, targetTr.position.y) + 2f;
        Vector3 midPoint = new Vector3((firstPos.x + targetTr.position.x) / 2f, midPointHeight, (firstPos.z + targetTr.position.z) / 2f);

        transform.DOMove(midPoint, 0.5f)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() =>
            {
                transform.Rotate(randomRotSpeed * Time.deltaTime);
            })
            .OnComplete(() =>
            {
                StartCoroutine(MoveTowardsTarget(firstPos, targetTr, randomRotSpeed));
            });
    }

    private IEnumerator MoveTowardsTarget(Vector3 startPos, Transform targetTr, Vector3 randomRotSpeed)
    {
        float counter = 0f;
        float duration = 0.15f;
        Vector3 initialPos = transform.position;

        while (counter < 1f)
        {
            if (targetTr == null) yield break;

            counter += Time.deltaTime / duration;

            Vector3 currentTargetPos = targetTr.position;
            Vector3 parabolicPos = Vector3.Lerp(initialPos, currentTargetPos, counter);

            float angle = counter * Mathf.PI;
            float parabolicHeight = Mathf.Sin(angle) * 2f;
            parabolicPos.y += parabolicHeight;

            transform.position = parabolicPos;
            transform.Rotate(randomRotSpeed * Time.deltaTime);

            yield return null;
        }

        if (targetTr != null)
        {
            transform.position = targetTr.position;
        }

        GameManager.Instance.MoneyUpdate(banknotValue);
        DeSpawn();
    }
    void DeSpawn()
    {
        banknotValue = 0;
        EZ_PoolManager.Despawn(this.transform);
    }
}

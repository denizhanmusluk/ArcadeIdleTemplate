using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class OpenElastic : MonoBehaviour
{
    [SerializeField] float firstSize, lastSize, duration;
    Vector3 firstScale;
    private void Awake()
    {
        firstScale = transform.localScale;
    }
    void OnEnable()
    {
        OpenScale(firstSize, lastSize, duration, Ease.OutElastic);
    }

    public Tween OpenScale(float value, float lastValue, float duration, DG.Tweening.Ease type)
    {

        Tween tween = DOTween.To
            (() => value, x => value = x, lastValue, duration).SetEase(type).OnUpdate(delegate ()
            {
                transform.localScale = firstScale * value;
            }).OnComplete(delegate ()
            {

            });
        return tween;
    }
}

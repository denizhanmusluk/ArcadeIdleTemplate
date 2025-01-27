using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackCollectMarketCustomer : StackCollect
{
    public int stackCapacity;
    public AIMarketCustomer _aiCustomer;
    public int targetId;
    public bool cabinActive = false;

    public override float StackFollowSpeed()
    {
        return _aiCustomer.aiMoving.aImoveSpeed * 4;
    }

    public override void StackAnimation()
    {
        _aiCustomer.animator.SetBool("carry", true);
        //if (collectionTrs[0].collectID == 0)
        //{
        //    StackIkPosSet(leftIkTarget, leftProductTarget, true);
        //    StackIkPosSet(rightIkTarget, rightProductTarget, true);
        //}
        //else
        //{
        //    StackIkPosSet(leftIkTarget, leftBoxTarget, true);
        //    StackIkPosSet(rightIkTarget, rightBoxTarget, true);
        //}
    }

    public override void StackEmptyAnimation()
    {
        _aiCustomer.animator.SetBool("carry", false);
        //StackIkPosSet(leftIkTarget, leftNullTarget, false);
        //StackIkPosSet(rightIkTarget, rightNullTarget, false);
    }



    void StackIkPosSet(Transform ikHandTR, Transform targetTR, bool ikActive)
    {
        ikLeft.enabled = true;
        ikRight.enabled = true;

        ikHandTR.DOMove(targetTR.position, 0.5f)
           .SetEase(Ease.Linear)
           .OnComplete(() =>
           {
               ikLeft.enabled = ikActive;
               ikRight.enabled = ikActive;
           });

        ikHandTR.DORotateQuaternion(targetTR.rotation, 0.5f)
            .SetEase(Ease.Linear);
    }

}

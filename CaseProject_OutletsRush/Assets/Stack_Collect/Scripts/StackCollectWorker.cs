using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackCollectWorker : StackCollect
{
    public int currentCapacity;
    public int baseStackCapacity;
    public AIWorker _aiWorker;

    public int targetId;
    public bool aiCollectActive = true;

    public CollectType collectableType;
    public bool carryPassive;
    public CharacterUpgradeSettings characterUpgradeSettings;

    private void Awake()
    {
        //characterUpgradeSettings = LevelManager.Instance._currnetCharacterUpgradeSettings;
    }
    public override float StackFollowSpeed()
    {
        return _aiWorker.aiMoving.aImoveSpeed;
    }
    public override void StackAnimation()
    {
        _aiWorker.animator.SetBool("carry", true);
        //StackIkPosSet(leftIkTarget, leftProductTarget, true);
        //StackIkPosSet(rightIkTarget, rightProductTarget, true);
    }
    public override void StackEmptyAnimation()
    {
        _aiWorker.animator.SetBool("carry", false);
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

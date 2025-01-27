using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackCollectPlayer : StackCollect
{
    public PlayerController _playerController;
    bool stackActive = true;
    public override void StackAnimation()
    {
        //_playerController.animator.SetBool("carry", true);
        if (!stackActive)
        {
            stackActive = true;
            StackIkPosSet(leftIkTarget, leftProductTarget, true);
            StackIkPosSet(rightIkTarget, rightProductTarget, true);
        }
    }

    public override void StackEmptyAnimation()
    {
        //_playerController.animator.SetBool("carry", false);
        if (stackActive)
        {
            stackActive = false;
            StackIkPosSet(leftIkTarget, leftNullTarget, false);
            StackIkPosSet(rightIkTarget, rightNullTarget, false);
        }
    }

    public override float StackFollowSpeed()
    {
        return _playerController._characterUpgradeSettings.characterSpeed[Globals.characterSpeedLevel];
    }
    void StackIkPosSet(Transform ikHandTR, Transform targetTR, bool ikActive)
    {
        ikLeft.enabled = true;
        ikRight.enabled = true;

        ikHandTR.DOLocalMove(targetTR.localPosition, 0.5f)
           .SetEase(Ease.Linear)
           .OnComplete(() =>
           {
            ikLeft.enabled = ikActive;
               ikRight.enabled = ikActive;
           });

        ikHandTR.DOLocalRotateQuaternion(targetTR.localRotation, 0.5f)
            .SetEase(Ease.Linear);
    }
}

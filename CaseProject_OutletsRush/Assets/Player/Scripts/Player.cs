using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class Player : MonoBehaviour
{
    public Animator _animator;
    public FastIKFabric ikLeft;
    public FastIKFabric ikRight;

    public Transform leftIkTarget;
    public Transform rightIkTarget;

    public Transform leftProductTarget;
    public Transform rightProductTarget;

    public Transform leftNullTarget;
    public Transform rightNullTarget;
    private void OnEnable()
    {
        EnableDelay();
        //Invoke("EnableDelay", 0.1f);
    }
    void EnableDelay()
    {
        PlayerController.Instance.animator = _animator;

        PlayerController.Instance._stackCollect.ikLeft = ikLeft;
        PlayerController.Instance._stackCollect.ikRight = ikRight;

        PlayerController.Instance._stackCollect.leftIkTarget = leftIkTarget;
        PlayerController.Instance._stackCollect.rightIkTarget = rightIkTarget;

        PlayerController.Instance._stackCollect.leftProductTarget = leftProductTarget;
        PlayerController.Instance._stackCollect.rightProductTarget = rightProductTarget;

        PlayerController.Instance._stackCollect.leftNullTarget = leftNullTarget;
        PlayerController.Instance._stackCollect.rightNullTarget = rightNullTarget;

        PlayerController.Instance._stackCollect.StackEmptyAnimation();
    }
}

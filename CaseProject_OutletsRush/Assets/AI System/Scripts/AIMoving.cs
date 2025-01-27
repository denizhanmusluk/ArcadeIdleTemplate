using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AIMoving : MonoBehaviour
{
    public delegate void FollowingTR(Transform posTR);
    public delegate void FollowingVector(Vector3 posVec);

    public event FollowingTR followingTR;
    public event FollowingVector followingVec;

    public event Action targetArrivedBehaviour;

    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public float aImoveSpeed;

    private void Start()
    {
        navMeshAgent.speed = aImoveSpeed;
    }
    public void MoveSpeedInit(float _moveSpeed)
    {
        navMeshAgent.speed = aImoveSpeed = _moveSpeed;
    }
    public void BehaviourInit(Action fnct)
    {
        targetArrivedBehaviour = null;
        targetArrivedBehaviour += fnct;
    }
    public void GoTargetStart(Transform targetPosTR, Action behaviour)
    {
        followingTR = null;
        targetArrivedBehaviour = null;
        targetArrivedBehaviour += behaviour;
        followingTR += (posTR) => GoToTarget(targetPosTR);
    }
    public void GoToTarget(Transform targetPosTR)
    {
        if (Vector3.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPosTR.position.x, targetPosTR.position.z)) > 0.35f)
        {
            Vector3 targetPos = new Vector3(targetPosTR.position.x, transform.position.y, targetPosTR.position.z);
            if (animator != null)
            {
                animator.SetBool("walk", true);
            }
            navMeshAgent.SetDestination(targetPos);
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("walk", false);
            }
            followingTR = null;
            targetArrivedBehaviour?.Invoke();

        }
    }
    public void GoTargetStart(Vector3 targetPos, Action behaviour)
    {
        followingVec = null;
        targetArrivedBehaviour = null;
        targetArrivedBehaviour += behaviour;

        followingVec += (posVec) => GoToTarget(targetPos);
    }
    public void GoToTarget(Vector3 targetPos)
    {
        if (Vector3.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetPos.x, targetPos.z)) > 0.35f)
        {
            Vector3 _targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            if (animator != null)
            {
                animator.SetBool("walk", true);
            }
            navMeshAgent.SetDestination(_targetPos);
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("walk", false);
            }
            followingVec = null;
            targetArrivedBehaviour?.Invoke();
        }
    }

  
    private void Update()
    {
        followingTR?.Invoke(transform);
        followingVec?.Invoke(Vector3.zero);
    }
}
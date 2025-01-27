using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Casier : MonoBehaviour
{
    public Animator _animator;
    private void OnEnable()
    {
        CheckoutManager.Instance.cashWorkerActive = true;
        CheckoutManager.Instance.activatorGO.SetActive(false);
    }
}
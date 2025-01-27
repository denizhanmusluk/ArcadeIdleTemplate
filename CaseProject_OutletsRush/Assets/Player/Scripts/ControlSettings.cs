using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Character/ControlSettings")]
public class ControlSettings : ScriptableObject
{
    [Header("Joy Stick")]

    [SerializeField] private float moveSpeed_Default = 1f;
    public float _moveSpeed_Default { get { return moveSpeed_Default; } }


    [Range(1.0f, 10.0f)]

    [SerializeField] private float rotateSensivity = 10f;
    public float _rotateSensivity { get { return rotateSensivity; } }
}


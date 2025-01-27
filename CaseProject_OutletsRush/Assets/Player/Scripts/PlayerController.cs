using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : Singleton<PlayerController>
{
    public static Action OnControl, OnUpdate, GoToCeo_Update;

    public CharacterUpgradeSettings _characterUpgradeSettings;
    public Player _player;
    public StackCollect _stackCollect;
    public CinemachineVirtualCamera PlayerCam;


    public Animator animator;
    [SerializeField] ControlSettings character_Settings;
    [SerializeField] ModelScriptable playerModels;
    public Transform characterSkinParent;
    [SerializeField] bool moveRotToCamera = true;


    Transform mainCameraTR;
    public bool dropActive = true;
    public MoneyPoint moneyPoint;
    public Transform moneyCollectTargetTR;

    public bool pressActive = false;
    public void CreateMoneyText(int mnyCount)
    {
        moneyPoint.gameObject.SetActive(true);
        moneyPoint.AnimationStart(mnyCount);
    }
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        mainCameraTR = Camera.main.transform;
        _characterUpgradeSettings = UpgradeManager.Instance._upgradeSettings;
        InitializeCameraFollowing();
        CreteSkin();
    }
    void InitializeCameraFollowing()
    {
        PlayerCam = CameraManager.Instance.vCamPlayer;
        PlayerCam.Follow = transform.parent;
        PlayerCam.LookAt = transform.parent;
    }
    void CreteSkin()
    {
        int selectSkin = UnityEngine.Random.Range(0, playerModels._skins.Length);
        var _playerSkin = Instantiate(playerModels._skins[selectSkin], transform.position, Quaternion.identity, characterSkinParent).GetComponent<Player>();
        CharacterSelect(_playerSkin);
    }
    public void CharacterSelect(Player _playerSkin)
    {
        if (_player != null)
        {
            Destroy(_player.gameObject);
            _player = null;
        }
        _player = _playerSkin;
        //_playerSkin.transform.parent = characterSkinParent;
        //_playerSkin.transform.localPosition = Vector3.zero;
        //_playerSkin.transform.localRotation = Quaternion.identity;
    }
    void Update()
    {
        OnUpdate?.Invoke();
        GoToCeo_Update?.Invoke();
    }
    public void GameStart()
    {
        OnUpdate += _Update;
        animator.SetBool("working", false);
        Globals.playerStackActive = true;
    }
    void _Update()
    {
        OnControl?.Invoke();
    }
    public void SwipeControl(Vector3 direction)
    {
        Vector3 moveDirection = direction;

        if (moveRotToCamera)
        {
            moveDirection = RotateSetForCamera(direction);
        }
        float moveSpeedValue = character_Settings._moveSpeed_Default * (_characterUpgradeSettings.characterSpeed[Globals.characterSpeedLevel]);
        transform.parent.transform.Translate(moveDirection * moveSpeedValue * Time.deltaTime);
        SetAnimationMoveSpeed(moveDirection, moveSpeedValue);
        float playerRot_y = PlayerYRotation(moveDirection);
        Quaternion newRot = Quaternion.Euler(0, playerRot_y, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, character_Settings._rotateSensivity * 60 * Time.deltaTime);


    }
    void SetAnimationMoveSpeed(Vector3 _direction, float _moveSpeed)
    {
        float animSpeedOffset = 0.125f;
        float moveSpeedFactor = _direction.magnitude;
        animator.SetFloat("MoveSpeed", moveSpeedFactor * (_moveSpeed * animSpeedOffset));

    }
    public void PlayerMoving()
    {
        pressActive = true;
        animator.SetBool("walk", true);
    }
    public void PlayerStop()
    {
        pressActive = false;
        animator.SetBool("walk", false);
    }
    Vector3 RotateSetForCamera(Vector3 _direction)
    {
        Quaternion rotationQuaternion = Quaternion.Euler(0, mainCameraTR.eulerAngles.y, 0);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);
        Vector3 rotatedDirection = rotationMatrix.MultiplyPoint3x4(_direction);
        return rotatedDirection;
    }
    float PlayerYRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        return angle;
    }



    public void DropActivator()
    {
        StartCoroutine(DropActivate());
    }
    IEnumerator DropActivate()
    {
        dropActive = false;
        yield return new WaitForSeconds(0.1f);
        dropActive = true;
    }
}
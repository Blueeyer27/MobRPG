using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Animator animator;

    public GameObject target;
    public Camera sceneCamera;

    public float runSpeed = 4;

    public WeaponType weaponType;
    public GameObject weapon;

    private bool canMove = true;
    private bool canAction = true;
    private bool isDead;
    public bool isMoving;

    private Vector3 startPosition;

    public Action<AIController> OnSelected;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //TODO: weaponType is not actually totally same as weaponNumber be careful...
        _UnSheathWeapon((int)weaponType);

        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (!canAction)
            return;

        FollowTarget();
    }

    private void FollowTarget()
    {
        if (isDead || !canMove || target == null)
        {
            if (Vector3.Distance(transform.position, startPosition) > 1f)
            {
                ReturnToStartPosition();
            }
            else
            {
                UpdateMoving(false);
            }

            return;
        }

        var targetPosition = target.transform.position;
        var distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        Debug.Log(distanceToTarget);

        if (distanceToTarget > 20f || Vector3.Distance(transform.position, startPosition) > 30f)
        {
            // AI lost target or ran too far away, let's return back
            target = null;
            return;
        }
        else if (distanceToTarget > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
            UpdateMoving(true);
        }
        else
        {
            UpdateMoving(false);
            Attack();
        }

        // Only in cases when we still interact with target (running/attack)
        if (target != null)
        {
            transform.LookAt(target.transform.position);
        }
    }

    private void ReturnToStartPosition()
    {
        if (Vector3.Distance(transform.position, startPosition) < 1f)
        {
            UpdateMoving(false);
            return;
        }

        transform.LookAt(startPosition);
        transform.position = Vector3.MoveTowards(transform.position, startPosition, runSpeed * Time.deltaTime);
    }

    private void UpdateMoving(bool move)
    {
        animator.SetFloat("Velocity X", 0);
        animator.SetFloat("Velocity Z", move ? 1 : 0);

        if (move != isMoving)
        {
            isMoving = move;
            animator.SetBool("Moving", isMoving);
        }
    }

    private void OnMouseDown()
    {
        if (OnSelected != null)
        {
            OnSelected(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ouch");
    }

    private void Attack()
    {
        animator.SetInteger("AttackSide", 2);

        int maxAttacks = 6;
        int attackNumber = UnityEngine.Random.Range(1, maxAttacks);
        animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");

        StartCoroutine(_Lock(true, true, true, 0, 1.5f));
    }

    public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (lockMovement)
        {
            //LockMovement();
        }
        if (lockAction)
        {
            //LockAction();
            canAction = false;
        }
        if (timed)
        {
            yield return new WaitForSeconds(lockTime);
            //UnLock(lockMovement, lockAction);
            canAction = true;
        }
    }

    public IEnumerator _UnSheathWeapon(int weaponNumber)
    {
        //two handed weapons
        if (weaponNumber < 7 || weaponNumber == 18 || weaponNumber == 20)
        {
            animator.SetInteger("LeftRight", 3);
            if (weaponNumber == 0)
            {
                weaponType = WeaponType.UNARMED;
            }
            if (weaponNumber == 1)
            {
                weaponType = WeaponType.TWOHANDSWORD;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.4f, true));
            }
            else if (weaponNumber == 2)
            {
                weaponType = WeaponType.TWOHANDSPEAR;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.5f, true));
            }
            else if (weaponNumber == 3)
            {
                weaponType = WeaponType.TWOHANDAXE;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.5f, true));
            }
            else if (weaponNumber == 4)
            {
                weaponType = WeaponType.TWOHANDBOW;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.55f, true));
            }
            else if (weaponNumber == 5)
            {
                weaponType = WeaponType.TWOHANDCROSSBOW;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.5f, true));
            }
            else if (weaponNumber == 6)
            {
                weaponType = WeaponType.STAFF;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
            }
            else if (weaponNumber == 18)
            {
                weaponType = WeaponType.RIFLE;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
            }
            else if (weaponNumber == 20)
            {
                weaponType = WeaponType.TWOHANDCLUB;
                StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
            }
            //if (!instantWeaponSwitch)
            //{
            animator.SetTrigger("WeaponUnsheathTrigger");
            //    StartCoroutine(_Lock(true, true, true, 0, 1.1f));
            //}
            //if (instantWeaponSwitch)
            //{
            //    StartCoroutine(_WeaponVisibility(weaponNumber, 0.2f, true));
            //    animator.SetTrigger("InstantSwitchTrigger");
            //}
        }

        if (weaponType == WeaponType.RIFLE)
        {
            animator.SetInteger("Weapon", 8);
        }
        else if (weaponType == WeaponType.TWOHANDCLUB)
        {
            animator.SetInteger("Weapon", 9);
        }
        else
        {
            animator.SetInteger("Weapon", weaponNumber);
        }

        yield return null;
    }

    public IEnumerator _WeaponVisibility(int weaponNumber, float delayTime, bool visible)
    {
        yield return new WaitForSeconds(delayTime);

        weapon.SetActive(visible);

        yield return null;
    }
}

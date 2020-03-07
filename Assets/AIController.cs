using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Animator animator;

    public GameObject target;
    public Camera sceneCamera;

    public float runSpeed = 5;

    public WeaponType weaponType;
    public GameObject weapon;

    private bool canMove = true;
    private bool canAction = true;
    private bool isDead;
    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //TODO: weaponType is not actually totally same ase weaponNumber be careful...
        _UnSheathWeapon((int)weaponType);

        animator.SetBool("Moving", true);
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

        if (target != null)
        {
            transform.LookAt(target.transform.position);
        }
    }

    private void FollowTarget()
    {
        if (isDead || !canMove || target == null)
        {
            if (isMoving)
            {
                animator.SetBool("Moving", false);
                isMoving = false;
            }

            return;
        }

        var targetPosition = target.transform.position;
        if (Vector3.Distance(transform.position, targetPosition) > 2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);

            var rb = GetComponent<Rigidbody>();
            //Get local velocity of charcter
            float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
            float velocityZel = transform.InverseTransformDirection(rb.velocity).z;

            Debug.Log("X: " + velocityXel / runSpeed);
            Debug.Log("Z: " + velocityZel / runSpeed);
            //Update animator with movement values
            animator.SetFloat("Velocity X", 0);
            animator.SetFloat("Velocity Z", 1);

            if (!isMoving)
            {
                animator.SetBool("Moving", true);
                isMoving = true;
            }
        }
        else
        {
            if (isMoving)
            {
                animator.SetBool("Moving", false);
                isMoving = false;
            }

            Attack();
        }
    }

    private void Attack()
    {
        animator.SetInteger("AttackSide", 2);

        int maxAttacks = 6;
        int attackNumber = Random.Range(1, maxAttacks);
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

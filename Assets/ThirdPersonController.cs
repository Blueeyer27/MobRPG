using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    public FixedJoystick leftJoystick;
    public FixedTouchField touchField;

    public Button attackButton;

    public PlayerController playerController;

    public Transform firePoint;
    public GameObject fireSkill;

    protected float CameraAngle;
    protected float CameraAngleSpeed = 0.2f;

    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        if (attackButton != null)
        {
            attackButton.onClick.AddListener(Attack);
        }

        if (playerController.leftWeapon != 4)
        {
            StartCoroutine(playerController._SwitchWeapon((int)WeaponType.TWOHANDBOW));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Control.m_Jump = Button.Pressed;
        playerController.inputHorizontal = leftJoystick.Horizontal;
        playerController.inputVertical = leftJoystick.Vertical;

        CameraAngle += touchField.TouchDist.x * CameraAngleSpeed;

        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(0, 4, -4);
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
    }

    public void Attack()
    {
        if (playerController.canAction && !playerController.isRelax && playerController.isGrounded && !playerController.isBlocking)
        {
            if (target != null)
            {
                transform.LookAt(target.transform);
            }

            //ATTACK LEFT
            if (playerController.weapon == WeaponType.SHIELD || playerController.weapon == WeaponType.RIFLE || playerController.weapon != WeaponType.ARMED || (playerController.weapon == WeaponType.ARMED && playerController.leftWeapon != 0) && playerController.leftWeapon != 7)
            {
                playerController.Attack(1);

                if (playerController.weapon == WeaponType.TWOHANDBOW)
                {
                    StartCoroutine(FireSkill());
                }
            }
            //ATTACK RIGHT
            else if (playerController.weapon == WeaponType.RIFLE || playerController.weapon != WeaponType.ARMED || (playerController.weapon == WeaponType.ARMED && playerController.rightWeapon != 0) || playerController.weapon == WeaponType.ARMEDSHIELD)
            {
                if (playerController.weapon != WeaponType.SHIELD)
                {
                    playerController.Attack(2);
                }
            }
        }
    }

    private IEnumerator FireSkill()
    {
        yield return new WaitForSeconds(0.5f);
        if (fireSkill == null || firePoint == null)
        {
            yield break;
        }

        var effect = Instantiate(fireSkill, firePoint.transform.position, transform.rotation);

        yield return new WaitForSeconds(1f);

        Destroy(effect);
    }
}

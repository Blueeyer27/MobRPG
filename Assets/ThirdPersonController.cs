using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public FixedJoystick leftJoystick;
    public FixedTouchField touchField;

    public PlayerController playerController;

    protected float CameraAngle;
    protected float CameraAngleSpeed = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        if (playerController.rightWeapon != 9)
        {
            StartCoroutine(playerController._SwitchWeapon(9));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Control.m_Jump = Button.Pressed;
        playerController.inputHorizontal = leftJoystick.Horizontal;
        playerController.inputVertical = leftJoystick.Vertical;

        CameraAngle += touchField.TouchDist.x * CameraAngleSpeed;

        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(0, 6, 8);
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
    }

    public void Attack()
    {
        if (playerController.canAction && !playerController.isRelax && playerController.isGrounded && !playerController.isBlocking)
        {
            //ATTACK RIGHT
            if (playerController.weapon == WeaponType.RIFLE || playerController.weapon != WeaponType.ARMED || (playerController.weapon == WeaponType.ARMED && playerController.rightWeapon != 0) || playerController.weapon == WeaponType.ARMEDSHIELD)
            {
                if (playerController.weapon != WeaponType.SHIELD)
                {
                    playerController.Attack(2);
                }
            }
        }
    }
}

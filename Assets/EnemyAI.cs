using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            StartCoroutine(playerController._SwitchWeapon((int)WeaponType.TWOHANDAXE));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

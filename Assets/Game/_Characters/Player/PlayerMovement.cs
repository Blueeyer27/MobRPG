using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI; // TODO consider re-wireing

namespace RPG.Characters {

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour {

        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint;
        AICharacterControl aiCharacterControl = null;
        GameObject walkTarget = null;

        void Start() {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.onMouseOverPotenyiallyWalkable += OnMouseOverPotenyiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy (Enemy enemy) {
            if (Input.GetMouseButton(0)) {
                aiCharacterControl.SetTarget(enemy.transform);
            }
        }

        void OnMouseOverPotenyiallyWalkable (Vector3 destination) {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1)) {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }

        // TODO make this get called again (controller support)
        void ProcessDirectMovement() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);
        }

    }
}
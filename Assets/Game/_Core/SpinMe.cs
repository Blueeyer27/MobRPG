using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
    public class SpinMe : MonoBehaviour {

        [SerializeField] float rotationSpeed;

        private void FixedUpdate() {
            transform.RotateAround(transform.position, transform.right, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters; 

namespace RPG.CameraUI {
    public class CameraRaycaster : MonoBehaviour {
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;

        const int POTENTIALLY_WALKABLELAYER = 8;
        float maxRaycastDepth = 100f;

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverPotenyiallyWalkable;

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        void Update() {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject()) {
                // Impliment UI ineraction
            } else {
                PreformRaycasts();
            }           
        }

        private void PreformRaycasts() {
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Specify layer priorities below, order matters
            if (RaycastForEnemy(ray)) { return; }
            if (RaycastForPotentiallyWalkable(ray)) { return; }
        }

        bool RaycastForEnemy(Ray ray) {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            if (hitInfo.collider != null) {
                var gameObjectHit = hitInfo.collider.gameObject;
                var enemyHit = gameObjectHit.GetComponent<Enemy>();
                if (enemyHit) {
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    onMouseOverEnemy(enemyHit);
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool RaycastForPotentiallyWalkable(Ray ray) {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLELAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit) {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverPotenyiallyWalkable(hitInfo.point);
                return true;

            }
            return false;
        }
    }
}
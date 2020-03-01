using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core; // TODO consider re-wire

namespace RPG.Weapons {
    public class Projectile : MonoBehaviour {

        [SerializeField] float projectileSpeed;
        [Header("Info: ")]
        [SerializeField] GameObject shooter; // So can inspected when paused

        const float DESTROY_DELAY = 0.1f;
        float damageCaused;

        public void SetShooter(GameObject shooter) {
            this.shooter = shooter;
        }

        public void SetDamage(float damage) {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed() {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision) {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer) {
                DamageIfDamageables(collision);
            }
        }

        private void DamageIfDamageables(Collision collision) {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            //Debug.Log("damageableComponent" + damageableComponent);
            if (damageableComponent) {
                (damageableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}

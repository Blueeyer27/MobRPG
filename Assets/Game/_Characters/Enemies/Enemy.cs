using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core; // TODO consider re-wire
using RPG.Weapons; // TODO consider re-wire

namespace RPG.Characters {

    public class Enemy : MonoBehaviour, IDamageable {

        [SerializeField] float maxHealthPoints;
        [SerializeField] float damagePerShot = 10f;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float firingPeriodInS = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        bool isAttacking = false;
        AICharacterControl aiCharacterControl = null;
        Player player = null;
        float currentHealthPoints;

        private void Start() {
            currentHealthPoints = maxHealthPoints;
            player = FindObjectOfType<Player>();
            aiCharacterControl = GetComponent<AICharacterControl>();
        }

        private void Update() {
            if (player.healthAsPercentage <= Mathf.Epsilon) {
                StopAllCoroutines();
                Destroy(this); // To stop enemy behaviour
            }

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= attackRadius && !isAttacking) {
                isAttacking = true;
                float randomisedDelay = Random.Range(firingPeriodInS - firingPeriodVariation, firingPeriodInS + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay); 
            }

            if (distanceToPlayer > attackRadius) {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius) {
                aiCharacterControl.SetTarget(player.transform);
            } else {
                aiCharacterControl.SetTarget(transform);
            }

        }

        // Get healthAsPercentage
        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        // IDamageable
        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            if (currentHealthPoints <= 0) {
                Destroy(this.gameObject);
            }
        }

        // TODO spearate out charater firing logic
        void FireProjectile() {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI; // TODO consider re-wireing
using RPG.Core; // TODO consider re-wire
using RPG.Weapons; // TODO consider re-wire
using UnityEngine.SceneManagement;

namespace RPG.Characters {
    public class Player : MonoBehaviour, IDamageable {

        [SerializeField] float maxHealthPoints;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [Range(0.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle = null;

        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        // Temporarily serializeing for debugging
        [SerializeField] AbilityConfig[] abilities;

        const string DEATH_TRIGGER = "Death";
        const string ATTACK_TRIGGER = "Attack";

        Enemy enemy = null;
        AudioSource audioSource = null;
        CameraRaycaster cameraRaycaster = null;
        Animator animator = null;
        float currentHealthPoints = 0f;
        float lastHitTime = 0f;

        private void Start() {
            audioSource = GetComponent<AudioSource>();

            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AttachInitialAbilities();
        }


        void Update() {
            if (healthAsPercentage > Mathf.Epsilon) {
                ScanForAbilityKeyDown();
            }
        }

        void AttachInitialAbilities() {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++) {
                abilities[abilityIndex].AttachComponentTo(gameObject);
            }
        }

        private void ScanForAbilityKeyDown() {
            for (int keyIndex = 1; keyIndex < abilities.Length; keyIndex++) {
                if (Input.GetKeyDown(keyIndex.ToString())) {
                    AttemptSpecialAbility(keyIndex);
                }
            }
        }

        public float healthAsPercentage {
            get { return currentHealthPoints / maxHealthPoints; }
        }


        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            audioSource.clip = damageSounds[Random.Range(0, damageSounds.Length)];
            audioSource.Play();
            if (currentHealthPoints <= 0) {          
                StartCoroutine(KillPlayer());
            } 
        }

        public void Heal(float points) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }


        IEnumerator KillPlayer() {
            audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            animator.SetTrigger(DEATH_TRIGGER);
            yield return new WaitForSecondsRealtime(audioSource.clip.length);
            SceneManager.LoadScene(0);
        }

        private void SetCurrentMaxHealth() {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator () {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // remove const
        }

        private void PutWeaponInHand() {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
            weapon.transform.localScale = weaponInUse.gripTransform.localScale;
        }

        private GameObject RequestDominantHand() {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one.");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHands scripts on Player, please remove one.");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick() {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void OnMouseOverEnemy (Enemy enemyToSet) {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject)) {
                AttackTarget();
            } else if (Input.GetMouseButtonDown(1)) {
                AttemptSpecialAbility(0);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex) {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if(energyComponent.IsEnergyAvailable(energyCost)) {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private void AttackTarget() {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits()) {
                animator.SetTrigger(ATTACK_TRIGGER);
                enemy.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage() {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + weaponInUse.GetAdditionalDamage();
            if (isCriticalHit) {
                criticalHitParticle.Play();
                return damageBeforeCritical * criticalHitMultiplier;
            } else {
                return damageBeforeCritical;
            }
        }

        private bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }
    }
}
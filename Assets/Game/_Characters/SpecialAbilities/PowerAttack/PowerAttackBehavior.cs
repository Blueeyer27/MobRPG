using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility {

        PowerAttackConfig config;
        ParticleSystem myParticleSystem;
        AudioSource audioSource;

        public void SetConfig(PowerAttackConfig configToSet) {
            this.config = configToSet;
        }

        void Start () {
            print("Power Attack behaviour attached to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams) {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}
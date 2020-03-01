using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility {

        SelfHealConfig config = null;
        ParticleSystem myParticleSystem;
        AudioSource audioSource = null;
        Player player = null;

        public void SetConfig(SelfHealConfig configToSet) {
            this.config = configToSet;
        }

        void Start() {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams) {
            player.Heal(config.GetExtraHealth());
            PlayParticleEffect();
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }

        private void PlayParticleEffect() {
            var particlePrefab = config.GetParticlePrefab();
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            prefab.transform.parent = transform;
            myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Effect"))]
    public class AreaEffectConfig : AbilityConfig {
        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 10f;

        public override void AttachComponentTo(GameObject gameObjectToattachTo) {
            var behaviourComponent = gameObjectToattachTo.AddComponent<AreaEffectBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float GetDamageToEachTarget() {
            return damageToEachTarget;
        }

        public float GetRadius() {
            return radius;
        }
    }
}
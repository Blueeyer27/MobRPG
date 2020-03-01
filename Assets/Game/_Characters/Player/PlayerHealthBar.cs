using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class PlayerHealthBar : MonoBehaviour {

        [SerializeField] Image healthOrb_Fill;
        [SerializeField] Image healthOrb_FillEmpty;

        Player player;

        void Start() {
            player = FindObjectOfType<Player>();
        }

        void Update() {
            healthOrb_Fill.fillAmount = player.healthAsPercentage;
            healthOrb_FillEmpty.fillAmount = 1 - player.healthAsPercentage;
        }
    }
}
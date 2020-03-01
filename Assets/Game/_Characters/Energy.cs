using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters {
    public class Energy : MonoBehaviour {

        [SerializeField] Image energyOrb_fill;
        [SerializeField] Image energyOrb_fillEmpty;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;

        float currentEnergyPoints;
       
        public bool IsEnergyAvailable(float amount) {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount) {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
        }

        private void Start() {
            currentEnergyPoints = maxEnergyPoints;
            UpdateEnergyBar();
        }

        private void Update() {
            if (currentEnergyPoints < maxEnergyPoints) {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void AddEnergyPoints() {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        private void UpdateEnergyBar() {
            energyOrb_fill.fillAmount = EnergyAsPercent();
            energyOrb_fillEmpty.fillAmount = 1 - EnergyAsPercent();
        }

        float EnergyAsPercent () {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Orbitality
{
    public class UI_CooldownBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private float _scaleMultiplier = 0.235f;
        private PlanetAI _planetReference;

        public void Initialize(PlanetAI planet)
        {
            _planetReference = planet;

            transform.SetParent(planet.transform);
            transform.localScale = planet.transform.localScale * _scaleMultiplier;
            transform.localPosition = Vector3.zero;

            _planetReference.OnShoot += Refresh;
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            SetFill(_planetReference.GetNormalizedCooldown());
        }

        public void SetFill(float normalized)
        {
            _fillImage.fillAmount = normalized;
        }
    }
}
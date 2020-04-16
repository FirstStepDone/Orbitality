using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Orbitality
{
    public class UI_HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private float _scaleMultiplier = 0.25f;
        private PlanetAI _planetReference;

        public void Initialize(PlanetAI planet)
        {
            _planetReference = planet;

            transform.SetParent(planet.transform);
            transform.localScale = planet.transform.localScale * _scaleMultiplier;
            transform.localPosition = Vector3.zero;

            _planetReference.OnDamaged += Refresh;
            _planetReference.OnHealthUpdated += Refresh;
        }

        private void Refresh()
        {
            SetFill(_planetReference.Health / _planetReference.InitialHealth);
        }

        public void SetFill(float normalized)
        {
            _fillImage.fillAmount = normalized;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality
{
    public class EnemyPlanetAI : PlanetAI
    {
        private readonly string projectilePhysicsLayer = "EnemyProjectile";

        private PlanetAI _currentTarget;
        private string _targetTag;
        private List<PlanetAI> _targets = new List<PlanetAI>();

        public override void Initialize()
        {
            CacheTargets();
            PickClosestTarget();

            if (_currentTarget == null)
            {
                Debug.LogWarning(this.name + ": No target found upon initialization!");
            }

            _shootTimestamp = Time.time + _weapon._cooldown;

            base.Initialize();
        }

        public override int GetType()
        {
            return 2;
        }

        public void SetTargetTag(string tag)
        {
            _targetTag = tag;
        }

        void CacheTargets()
        {
            List<PlanetAI> planets = new List<PlanetAI>();
            planets.AddRange(FindObjectsOfType<PlanetAI>());

            for (int i = 0; i < planets.Count; i++)
                if (planets[i].gameObject.tag.Equals(_targetTag))
                    _targets.Add(planets[i]);
        }

        void PickClosestTarget()
        {
            float minDistance = float.MaxValue;
            PlanetAI mostSuitableTarget = null;

            for (int i = 0; i < _targets.Count; i++)
            {
                if (_targets[i] == null)
                    continue;

                if (_targets[i].Health <= 0)
                    continue;

                float distance = Vector3.Distance(transform.position, _targets[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    mostSuitableTarget = _targets[i];
                }
            }

            _currentTarget = mostSuitableTarget;
        }

        private void Update()
        {
            if (!IsActive)
                return;

            if (_currentTarget != null && _currentTarget.Health > 0)
            {
                if (CanShoot())
                {
                    Shoot((_currentTarget.transform.position - transform.position).normalized, projectilePhysicsLayer);
                }
            }
            else
            {
                PickClosestTarget();

                if (_currentTarget == null)
                    SetState(false);
            }
        }
    }
}
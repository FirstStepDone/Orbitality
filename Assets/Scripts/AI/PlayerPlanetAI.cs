using UnityEngine;

namespace Orbitality
{
    public class PlayerPlanetAI : PlanetAI
    {
        private readonly string projectilePhysicsLayer = "PlayerProjectile";

        public override void Initialize()
        {
            _shootTimestamp = Time.time + _weapon._cooldown;
            base.Initialize();
        }

        public override int GetType()
        {
            return 1;
        }

        public void Shoot(Vector3 direction)
        {
            Shoot(direction, projectilePhysicsLayer);
        }
    }
}
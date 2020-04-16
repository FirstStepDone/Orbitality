using UnityEngine;

namespace Orbitality
{
    public class PlanetAI : EntityAI, ITaget
    {

        //Events
        public event System.Action OnHealthUpdated;
        public event System.Action OnOutOfHealth;
        public event System.Action OnDamaged;
        public event System.Action OnShoot;

        //Properties
        public float Health
        {
            get { return _health; }
        }
        private float _health;

        public float InitialHealth
        {
            get { return _initialHealth; }
        }
        private float _initialHealth;

        //Variables
        protected Weapon _weapon;
        protected float _shootTimestamp = -1f;
        protected float projectileSpawnOffsetRadius = 0.5f;

        //Methods
        public override void Initialize()
        {
            SetState(true);
        }

        public override int GetType()
        {
            return 3;
        }

        public void Hit(float damage)
        {
            _health -= damage;
            _health = Mathf.Max(0, _health);

            OnDamaged.Fire();

            if (_health <= 0)
            {
                OnOutOfHealth.Fire();
                SetState(false);
                Destroy();
            }
        }

        public void SetHealth(float value)
        {
            _health = value;
            OnHealthUpdated.Fire();
        }

        public void SetMaxHealth(float value)
        {
            _initialHealth = value;
            OnHealthUpdated.Fire();
        }

        public void SetSize(float value)
        {
            transform.localScale = Vector3.one * value;
        }

        public void SetWeapon(Weapon newWeapon)
        {
            _weapon = newWeapon;
        }

        public void Shoot(Vector3 direction, string physicsLayer = "Default")
        {
            if (!IsActive)
                return;

            if (Health > 0)
            {
                if (Time.time > _shootTimestamp)
                {
                    Vector3 projectileSpawnPoint = transform.position + direction * projectileSpawnOffsetRadius;
                    ProjectileSpawner.SpawnProjectile(_weapon._projectile, projectileSpawnPoint, direction, physicsLayer);
                    _shootTimestamp = Time.time + _weapon._cooldown;
                }
            }
        }

        public bool CanShoot()
        {
            return Time.time > _shootTimestamp && Health > 0 && IsActive;
        }

        public float GetNormalizedCooldown()
        {
            return Mathf.Clamp(1 - ((_shootTimestamp - Time.time) / _weapon._cooldown), 0, 1);
        }

    }
}
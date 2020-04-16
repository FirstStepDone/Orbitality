using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        //Events
        public event System.Action<GameObject> OnDestroyed;

        //Variables
        public int typeID;

        //Dependencies
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ProjectileSettings _projectileSettings;

        public int GetTypeID()
        {
            return typeID;
        }

        //IProjectile method that is used to initialize projectile features based on the given direction
        //(presumably most of the time means shot direction)
        public void Initialize(Vector3 direction)
        {
            _rigidbody.velocity = direction.normalized * _projectileSettings.initialSpeed;
        }

        //IProjectile method for controlling projectile's state
        public void Deactivate()
        {
            OnDestroyed.Fire(this.gameObject);
            Destroy(this.gameObject);
        }

        private void Update()
        {
            if (IsOutOfRange())
                Deactivate();
        }

        private void LateUpdate()
        {
            UpdateRotation();
        }

        private bool IsOutOfRange()
        {
            return (Vector3.Distance(transform.position, Vector3.zero) > 30f);
        }

        private void UpdateRotation()
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.up);
        }

        private void FixedUpdate()
        {
            Vector3 accelerationVector = (transform.rotation * Vector3.forward).normalized;
            _rigidbody.AddForce(accelerationVector * _projectileSettings.acceleration, ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision collision)
        {
            ITaget target = collision.collider.GetComponent<ITaget>();

            if (target != null)
                target.Hit(_projectileSettings.damage);

            Deactivate();
        }

        public void SubscribeToUponDeathEvent(System.Action<GameObject> action)
        {
            OnDestroyed += action;
        }
    }
}
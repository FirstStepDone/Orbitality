using UnityEngine;

/*
    Class that handles gravitation. Why not use real world force of gravity in the
    small 2D arcade game? There is no reason not to.. :P
*/

namespace Orbitality
{
    public class PhysicalObject : MonoBehaviour
    {
        [SerializeField] private Rigidbody _body;
        [SerializeField] private float _mass;
        [SerializeField] private float _initialAcceleration;

        public void SetInitialAcceleration(float value)
        {
            _initialAcceleration = value;
        }

        void Start()
        {
            //Assuming that 'sun' is in the middle of a scene
            Vector3 dirToSun = (Vector3.zero - transform.position).normalized;
            Vector3 initialAccelerationDirection = Vector3.Cross(dirToSun, -Vector3.up);

            //If there is any acceleration should be applied
            if (_initialAcceleration > 0)
                _body.AddForce(initialAccelerationDirection.normalized * _initialAcceleration / 8f, ForceMode.Acceleration);

            //Update RB mass
            _body.mass = _mass;
        }

        //Determine the force with which other objects (every object in the 'Universe') pull towards them
        void FixedUpdate()
        {
            Vector3 acceleration = Vector3.zero;

            //Array with all objects cached
            for (int i = 0; i < Universe.allPhysicalObjects.Length; i++)
            {
                PhysicalObject obj = Universe.allPhysicalObjects[i];

                if (obj.GetInstanceID() == this.GetInstanceID())
                    continue;

                Vector3 gravityDirection = (obj.transform.position - this.transform.position).normalized;

                float m = obj._mass;

                float r = Vector3.Distance(transform.position, obj.transform.position) * 8f;
                r = Mathf.Clamp(r, 0.00000001f, float.MaxValue);

                float a = (m / Mathf.Pow(r, 2)) * 0.0005f;

                Vector3 gravityForce = gravityDirection * a;

                //Accumulate acceleration vector
                acceleration += gravityForce;
            }

            //Combine current velocity with gravity
            _body.AddForce(acceleration, ForceMode.Acceleration);
        }
    }
}
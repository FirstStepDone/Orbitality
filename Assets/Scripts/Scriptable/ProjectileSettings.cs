using UnityEngine;

/*
    Projectiles parameters encapsulated in a Scriptable container/file for optimization and reuse purpose
    In case of object pooling eases runtime back and forth testing
*/

namespace Orbitality
{
    [CreateAssetMenu(menuName = "Orbitality/ProjectileSettings", fileName = "ProjectileSettings")]
    public class ProjectileSettings : ScriptableObject
    {
        public float acceleration;
        public float initialSpeed;
        public float damage;
        [Range(0, 5)]
        public float gravityEffectiveness;

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality
{
    [CreateAssetMenu(menuName = "Orbitality/Weapon", fileName = "New Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] public GameObject _projectile;
        [SerializeField] public float _cooldown = 5f;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality
{
    public static class ProjectileSpawner
    {

        public static List<GameObject> registeredProjectiles = new List<GameObject>();

        public static GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 position, Vector3 shotDirection)
        {
            GameObject instance = GameObject.Instantiate(projectilePrefab, position, Quaternion.LookRotation(shotDirection, Vector3.up));
            instance.SetActive(true);

            IProjectile projectile = instance.GetComponent<IProjectile>();
            projectile.Initialize(shotDirection);
            projectile.SubscribeToUponDeathEvent(UnRegisterProjectile);

            RegisterProjectile(instance);

            return instance;
        }

        public static GameObject SpawnProjectile(GameObject projectilePrefab, Vector3 position, Vector3 rotation, Vector3? velocity = null)
        {
            GameObject instance = GameObject.Instantiate(projectilePrefab, position, Quaternion.Euler(rotation));
            instance.SetActive(true);

            IProjectile projectile = instance.GetComponent<IProjectile>();
            projectile.SubscribeToUponDeathEvent(UnRegisterProjectile);

            instance.GetComponent<Rigidbody>().velocity = velocity ?? Vector3.zero;

            RegisterProjectile(instance);

            return instance;
        }

        public static void SpawnProjectile(GameObject projectilePrefab, Vector3 position, Vector3 shotDirection, string layer)
        {
            GameObject instance = SpawnProjectile(projectilePrefab, position, shotDirection);

            instance.layer = LayerMask.NameToLayer(layer);
            for (int i = 0; i < instance.transform.childCount; i++)
                instance.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(layer);
        }

        public static void RegisterProjectile(GameObject projectileGameObject)
        {
            registeredProjectiles.Add(projectileGameObject);
        }

        public static void UnRegisterProjectile(GameObject projectileGameObject)
        {
            registeredProjectiles.Remove(projectileGameObject);
        }

        public static void ResesProjectileRegister()
        {
            registeredProjectiles.Clear();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Does not implement state of enemy shoot cooldown as the current time measuring is based on Unity's Time.time
*/
namespace Orbitality
{
    public class SkirmishLoadSystem : LoadSystem
    {
        protected override LevelData levelData
        {
            get { return skirmishLevelData; }
        }
        private SkirmishLevelData skirmishLevelData = new SkirmishLevelData();

        [SerializeField] private SkirmishController _gameController;
        [SerializeField] private EntitiesSpawner _spawner;

        void Start()
        {
            OnGameSaved += () =>
            {
                CleanupData();
            };

            OnGameLoadCompleted += (bool success) =>
            {
                CleanupData();
            };
        }

        public override void InitializeLoadedData()
        {
            _gameController.SetDiffitultySettings(skirmishLevelData.difficulty);
            _spawner.UpdateSettings(_gameController.activeLevelSettings);

            for (int i = 0; i < skirmishLevelData.entityData.Count; i++)
            {
                _spawner.InstantiatePlanet(
                    skirmishLevelData.entityData[i].typeId,
                    skirmishLevelData.entityData[i].visualId,
                    skirmishLevelData.entityData[i].size,
                    skirmishLevelData.entityData[i].health,
                    skirmishLevelData.entityData[i].initialHealth,
                    skirmishLevelData.entityData[i].position,
                    0,
                    skirmishLevelData.entityData[i].velocity
                );
            }

            for (int i = 0; i < skirmishLevelData.projectileData.Count; i++)
            {
                int projectileType = skirmishLevelData.projectileData[i].typeId;
                GameObject projectile = null;

                bool success = _spawner.projectileCatalog.TryGetValue(projectileType, out projectile);
                if (success)
                {
                    ProjectileSpawner.SpawnProjectile(
                        projectile,
                        skirmishLevelData.projectileData[i].position,
                        skirmishLevelData.projectileData[i].rotation,
                        skirmishLevelData.projectileData[i].velocity
                    );
                }
                else
                {
                    Debug.LogError(this.name + ": No such projectile found, projectile type ID: " + projectileType);
                }
            }

            _spawner.InitializeEntitiesBehaviour();
        }

        public override void PrepareSaveData()
        {
            skirmishLevelData.difficulty = _gameController.Difficulty;

            PullEntityData();
            PullProjectileData();
        }

        private void PullEntityData()
        {
            List<PlanetAI> entities = new List<PlanetAI>();
            entities.AddRange(FindObjectsOfType<PlanetAI>());

            for (int i = 0; i < entities.Count; i++)
            {
                skirmishLevelData.entityData.Add
                (
                    new SkirmishLevelData.EntityData
                    {
                        typeId = entities[i].GetType(),
                        visualId = entities[i].GetComponent<PlanetRenderer>().VisualID,
                        size = entities[i].transform.localScale.x,
                        position = entities[i].transform.position,
                        velocity = entities[i].GetComponent<Rigidbody>().velocity,
                        health = entities[i].Health,
                        initialHealth = entities[i].InitialHealth
                    }
                );
            }
        }

        private void PullProjectileData()
        {
            List<GameObject> projectiles = ProjectileSpawner.registeredProjectiles;
            for (int i = 0; i < projectiles.Count; i++)
            {
                skirmishLevelData.projectileData.Add
                (
                    new SkirmishLevelData.ProjectileData
                    {
                        typeId = projectiles[i].GetComponent<IProjectile>().GetTypeID(),
                        position = projectiles[i].transform.position,
                        rotation = projectiles[i].transform.rotation.eulerAngles,
                        velocity = projectiles[i].GetComponent<Rigidbody>().velocity,
                    }
                );
            }
        }

        public override void ParseSaveData(string json, System.Action<bool> OnComplete = null)
        {
            bool success = false;
            try
            {
                skirmishLevelData = JsonUtility.FromJson<SkirmishLevelData>(json);
                success = true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(this.name + ": Exception occured during load: " + ex.Message);
                success = false;
            }
            finally
            {
                OnComplete.Fire(success);
            }
        }

        public override void CleanGamestate()
        {
            List<EntityAI> entities = new List<EntityAI>();
            entities.AddRange(FindObjectsOfType<EntityAI>());
            for (int i = 0; i < entities.Count; i++)
                Destroy(entities[i].gameObject);

            List<GameObject> projectiles = ProjectileSpawner.registeredProjectiles;
            for (int i = 0; i < projectiles.Count; i++)
                Destroy(projectiles[i].gameObject);

            ProjectileSpawner.ResesProjectileRegister();
        }

        public void CleanupData()
        {
            skirmishLevelData.entityData.Clear();
            skirmishLevelData.projectileData.Clear();
        }

        [System.Serializable]
        public class SkirmishLevelData : LevelData
        {
            public int difficulty = 0;
            public List<EntityData> entityData = new List<EntityData>();
            public List<ProjectileData> projectileData = new List<ProjectileData>();

            [System.Serializable]
            public class EntityData
            {
                public int typeId;
                public int visualId;
                public float size;
                public Vector3 position;
                public Vector3 velocity;
                public float health;
                public float initialHealth;
            }

            [System.Serializable]
            public class ProjectileData
            {
                public int typeId;
                public Vector3 position;
                public Vector3 rotation;
                public Vector3 velocity;
            }
        }
    }
}
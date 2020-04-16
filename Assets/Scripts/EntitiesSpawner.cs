using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
    Class that handles objects initialization in a level, based on passed set of parameters
    Also handles initialization after loading
*/

namespace Orbitality
{
    public class EntitiesSpawner : MonoBehaviour
    {
        public GameObject basePlanetPrefab;
        public Dictionary<int, GameObject> projectileCatalog = new Dictionary<int, GameObject>();
        [SerializeField] private BehaviourBuilder _playerBuilder;
        [SerializeField] private BehaviourBuilder _enemyBuilder;
        [SerializeField] private BehaviourBuilder _neutralBuilder;
        private LevelSettings _activeSettings;

        public void Initialize(LevelSettings setting)
        {
            UpdateSettings(setting);
            SetProjectileCatalog();
            SpawnEntities();
            InitializeEntitiesBehaviour();
        }

        public void UpdateSettings(LevelSettings setting)
        {
            _activeSettings = setting;
            SetProjectileCatalog();
        }

        //Going through total count of entities, determening concrete type that will be spawned in the following order
        //1. Allies (only player at the moment), 2. Enemies, 3. Neutral planets
        public void SpawnEntities()
        {
            int enemyCount = Random.Range(_activeSettings.minEnemyCount, _activeSettings.maxEnemyCount + 1);
            int neutral = _activeSettings.neutralPlanetsCount;
            int allies = 1;

            int totalEntitiesToSpawn = allies + enemyCount + neutral;

            //Planet size variables
            float minSize = _activeSettings.minPlanetSize;
            float maxSize = _activeSettings.maxPlanetSize;

            for (int i = 0; i < totalEntitiesToSpawn; i++)
            {
                //Determining offset position from a sun
                //Making sure that planets do not overlap each others orbits
                float pos = 3f * (float)i / (float)totalEntitiesToSpawn;

                //Since the gravity is based on real world physics
                //determining initial velocity that so the planets stick to their orbits
                //and their full cycle rotation shape distributed more evenly
                float initialAcceleration = 700f + 30f * (float)i / (float)totalEntitiesToSpawn;

                //Determening spawn position
                Vector2 random = Random.insideUnitCircle;
                Vector3 randomDirectionOfSpawn = new Vector3(random.x, 0, random.y).normalized;
                Vector3 spawnPosition = randomDirectionOfSpawn * 3f + randomDirectionOfSpawn * pos;
                int visualType = Random.Range(1, _activeSettings.planetSpritesCatalog.Count + 1);
                float size = Random.Range(minSize, maxSize);

                int type = 0;
                float health = 0;

                //Health based on entity type, retrieved from settings
                if (allies > 0)
                {
                    type = 1;
                    health = _activeSettings.playerHealth;
                    allies--;
                }
                else if (enemyCount > 0)
                {
                    type = 2;
                    health = _activeSettings.enemyHealth;
                    enemyCount--;
                }
                else if (neutral > 0)
                {
                    type = 3;
                    health = _activeSettings.neutralPlanetHealth;
                    neutral--;
                }

                InstantiatePlanet(type, visualType, size, health, health, spawnPosition, initialAcceleration);
            }
        }

        public void InstantiatePlanet(int type, int visualType, float size, float health, float maxHealth, Vector3 spawnPosition, float initialAcceleration, Vector3? velocity = null)
        {
            GameObject instance = Instantiate(basePlanetPrefab, spawnPosition, Quaternion.identity);

            instance.GetComponent<PlanetRenderer>().SetMainBody(_activeSettings.planetSpritesCatalog[visualType - 1], visualType);
            
            instance.GetComponent<Rigidbody>().velocity = velocity ?? Vector3.zero;
            instance.GetComponent<PhysicalObject>().SetInitialAcceleration(initialAcceleration);

            if (type == 1)
                _playerBuilder.CreateBehaviour(instance);
            else if (type == 2)
                _enemyBuilder.CreateBehaviour(instance);
            else if (type == 3)
                _neutralBuilder.CreateBehaviour(instance);

            PlanetAI ai = instance.GetComponent<PlanetAI>();
            ai.SetHealth(health);
            ai.SetMaxHealth(maxHealth);
            ai.SetSize(size);
        }

        public void InitializeEntitiesBehaviour()
        {
            List<EntityAI> entities = new List<EntityAI>();
            entities.AddRange(FindObjectsOfType<EntityAI>());
            entities.ForEach((entity) => entity.Initialize());
        }

        public void SetProjectileCatalog()
        {
            for (int i = 0; i < _activeSettings.weaponCatalog.Count; i++)
            {
                int typeID = _activeSettings.weaponCatalog[i]._projectile.GetComponent<IProjectile>().GetTypeID();
                if (!projectileCatalog.ContainsKey(typeID))
                {
                    projectileCatalog.Add(typeID, _activeSettings.weaponCatalog[i]._projectile);
                }
            }
        }
    }
}
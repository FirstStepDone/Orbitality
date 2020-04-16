using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality
{
    [CreateAssetMenu(menuName = "Orbitality/LevelSettings", fileName = "New LevelSettings")]
    public class LevelSettings : ScriptableObject
    {
        public int minEnemyCount = 1;
        public int maxEnemyCount = 4;
        public int enemyHealth = 100;
        public int playerHealth = 250;
        public int neutralPlanetHealth = 1000;
        public int neutralPlanetsCount = 2;
        public float minPlanetSize = 0.3f;
        public float maxPlanetSize = 1f;
        public List<Weapon> weaponCatalog;
        public List<Sprite> planetSpritesCatalog;
    }
}
using UnityEngine;

namespace Orbitality
{
    public class SimpleEnemyBuilder : BehaviourBuilder
    {
        [SerializeField] private SkirmishController _gameController;
        [SerializeField] private GameObject _enemyHealthBar;
        [SerializeField] private GameObject _enemyCooldownBar;

        public override PlanetAI CreateBehaviour(GameObject entity)
        {
            entity.layer = LayerMask.NameToLayer("Enemy");
            entity.tag = "Enemy";

            EnemyPlanetAI ai = entity.AddComponent<EnemyPlanetAI>();

            Weapon randomWeapon = _gameController.activeLevelSettings.weaponCatalog[Random.Range(0, _gameController.activeLevelSettings.weaponCatalog.Count)];

            ai.SetWeapon(randomWeapon);
            ai.SetTargetTag("Player");

            GameObject healthbarInstance = Instantiate(_enemyHealthBar, transform.position, Quaternion.identity);
            healthbarInstance.GetComponent<UI_HealthBar>().Initialize(ai);

            GameObject cooldownInstance = Instantiate(_enemyCooldownBar, transform.position, Quaternion.identity);
            cooldownInstance.GetComponent<UI_CooldownBar>().Initialize(ai);

            return ai;
        }
    }
}
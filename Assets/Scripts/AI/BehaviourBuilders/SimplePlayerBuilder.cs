using UnityEngine;

namespace Orbitality
{
    public class SimplePlayerBuilder : BehaviourBuilder
    {
        [SerializeField] private SkirmishController _gameController;
        [SerializeField] private PlayerShootingController _playerController;
        [SerializeField] private GameObject _playerHealthBar;
        [SerializeField] private GameObject _playerCooldownBar;

        public override PlanetAI CreateBehaviour(GameObject entity)
        {
            PlayerPlanetAI ai = entity.AddComponent<PlayerPlanetAI>();

            entity.layer = LayerMask.NameToLayer("Player");
            entity.tag = "Player";

            Weapon randomWeapon = _gameController.activeLevelSettings.weaponCatalog[Random.Range(0, _gameController.activeLevelSettings.weaponCatalog.Count)];
            ai.SetWeapon(randomWeapon);

            _playerController.AssignPlayerPlanet(ai);

            GameObject healthbarInstance = Instantiate(_playerHealthBar, transform.position, Quaternion.identity);
            healthbarInstance.GetComponent<UI_HealthBar>().Initialize(ai);

            GameObject cooldownInstance = Instantiate(_playerCooldownBar, transform.position, Quaternion.identity);
            cooldownInstance.GetComponent<UI_CooldownBar>().Initialize(ai);

            return ai;
        }
    }
}
using UnityEngine;

namespace Orbitality
{
    public class PlayerShootingController : MonoBehaviour
    {
        [SerializeField] private InputController _inputController;
        private PlayerPlanetAI _playerPlanet;

        private void Awake()
        {
            _inputController.OnMouseButtonDown += () =>
            {
                if (_playerPlanet != null)
                    RequestShootProjectile();
            };
        }

        private void RequestShootProjectile()
        {
            if (_playerPlanet.CanShoot())
            {
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 shotDirection = worldMousePosition - _playerPlanet.transform.position;
                shotDirection.y = 0;
                shotDirection.Normalize();
                _playerPlanet.Shoot(shotDirection);
            }
        }

        public void AssignPlayerPlanet(PlayerPlanetAI playerPlanet)
        {
            _playerPlanet = playerPlanet;
        }
    }
}
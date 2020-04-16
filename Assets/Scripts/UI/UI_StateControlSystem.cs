using UnityEngine;

namespace Orbitality
{
    public class UI_StateControlSystem : MonoBehaviour
    {
        //Dependencies
        [SerializeField] private LoadSystem _loadSystem;
        [SerializeField] private SkirmishController _gameController;

        //References
        [SerializeField] private UI_State _initialState;
        [SerializeField] private UI_State _menuState;
        [SerializeField] private UI_State _gameState;
        [SerializeField] private UI_State _pauseState;

        private UI_State _currentState;

        void Awake()
        {
            _loadSystem.OnGameLoadCompleted += (success) =>
            {
                if (success)
                    ChangeState(_gameState);
            };
            _gameController.OnNewGameStarted += () =>
            {
                ChangeState(_gameState);
            };
            _gameController.OnPause += () =>
            {
                ChangeState(_pauseState);
            };
            _gameController.OnResume += () =>
            {
                ChangeState(_gameState);
            };
            _gameController.OnExitSession += () =>
            {
                ChangeState(_menuState);
            };
        }

        void Start()
        {
            ChangeState(_initialState);
        }

        void ChangeState(UI_State newState)
        {
            if (_currentState != null)
                _currentState.Hide();

            newState.Show();
            _currentState = newState;
        }

    }
}
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

/*
    Game mode runner class
    Lazy implementation of pause/resume feature
*/
namespace Orbitality
{
    public class SkirmishController : MonoBehaviour
    {
        //Events
        public event Action OnPause;
        public event Action OnResume;
        public event Action<int> OnDifficultyChanged;
        public event Action OnNewGameStarted;
        public event Action OnExitSession;

        //Properties
        public int Difficulty
        {
            get { return _difficulty; }
            set { _difficulty = Mathf.Clamp(value, 1, _settingsCatalog.Count); }
        }
        [SerializeField] protected int _difficulty;

        //Variables
        [HideInInspector] public LevelSettings activeLevelSettings;
        [SerializeField] private LoadSystem _loadSystem;
        [SerializeField] private EntitiesSpawner _spawner;
        [SerializeField] private List<LevelSettings> _settingsCatalog;

        public void IncreaseDifficulty()
        {
            ChangeDifficulty(true);
        }

        public void DecreaseDifficulty()
        {
            ChangeDifficulty(false);
        }

        public void ChangeDifficulty(bool increase)
        {
            _difficulty += increase ? 1 : -1;
            _difficulty = Mathf.Clamp(_difficulty, 1, _settingsCatalog.Count);

            OnDifficultyChanged.Fire(_difficulty);
        }

        public void Pause()
        {
            Time.timeScale = 0;

            OnPause.Fire();
        }

        public void Resume()
        {
            Time.timeScale = 1;

            OnResume.Fire();
        }

        public void StartNewGame()
        {
            SetDiffitultySettings(_difficulty);

            _spawner.Initialize(activeLevelSettings);
            _spawner.InitializeEntitiesBehaviour();

            ResetTimescale();

            OnNewGameStarted.Fire();
        }

        public void SetDiffitultySettings(int difficultyId)
        {
            activeLevelSettings = _settingsCatalog[difficultyId - 1];
        }

        public void ExitGameSession()
        {
            _loadSystem.CleanGamestate();

            ResetTimescale();

            OnExitSession.Fire();
        }

        public void ResetTimescale()
        {
            Time.timeScale = 1;
        }

        private void OnValidate()
        {
            _difficulty = Mathf.Clamp(_difficulty, 1, _settingsCatalog.Count);
        }
    }
}
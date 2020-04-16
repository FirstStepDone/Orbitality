using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Orbitality
{
    [RequireComponent(typeof(Text))]
    public class UI_CurrentDifficultyInfo : MonoBehaviour
    {
        [SerializeField] private SkirmishController _gameController;
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
            UpdateText(_gameController.Difficulty);
        }

        private void OnEnable()
        {
            _gameController.OnDifficultyChanged += UpdateText;
        }

        private void OnDisable()
        {
            _gameController.OnDifficultyChanged -= UpdateText;
        }

        public void UpdateText(int difficulty)
        {
            _text.text = "Difficulty " + difficulty.ToString();
        }
    }
}
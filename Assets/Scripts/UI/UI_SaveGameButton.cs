using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Orbitality
{
    public class UI_SaveGameButton : MonoBehaviour
    {
        [SerializeField] private LoadSystem _loadSystem;
        private Button _button;

        void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Load);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Load);
        }

        private void Load()
        {
            _loadSystem.Save();
        }
    }
}
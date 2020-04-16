using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality
{
    public class UI_State : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        public void Show()
        {
            _content.SetActive(true);
        }

        public void Hide()
        {
            _content.SetActive(false);
        }
    }
}
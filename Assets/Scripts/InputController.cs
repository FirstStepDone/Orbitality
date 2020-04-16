using UnityEngine;

namespace Orbitality
{
    public class InputController : MonoBehaviour
    {
        public event System.Action OnMouseButtonDown;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseButtonDown.Fire();
        }
    }
}
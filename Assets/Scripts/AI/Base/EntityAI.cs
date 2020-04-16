using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitality
{
    public abstract class EntityAI : MonoBehaviour
    {

        //Events
        public event System.Action OnInitialized;
        public event System.Action<bool> OnStateChanged;

        //Properties
        public bool IsActive
        {
            get { return _isActive; }
        }
        bool _isActive = false;

        //Methods
        public abstract void Initialize();
        public abstract int GetType();

        public void SetState(bool isActive)
        {
            _isActive = isActive;
            OnStateChanged.Fire(isActive);
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
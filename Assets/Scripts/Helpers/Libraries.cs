using UnityEngine;

namespace Orbitality
{
    public interface IProjectile
    {
        void Initialize(Vector3 forward);
        void Deactivate();
        int GetTypeID();
        void SubscribeToUponDeathEvent(System.Action<GameObject> method);
    }

    public interface ITaget
    {
        void Hit(float damage);  
    }

    public interface ISpaceObject
    {
        
    }
}
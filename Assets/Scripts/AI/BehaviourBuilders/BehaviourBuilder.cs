using UnityEngine;
/*
    Base class-factory for creating different behaviours
*/
namespace Orbitality
{
    public abstract class BehaviourBuilder : MonoBehaviour
    {
        //Instance passed as a method parameter, new behaviour attached to that instance
        public abstract PlanetAI CreateBehaviour(GameObject instance);
    }
}
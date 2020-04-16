using UnityEngine;

namespace Orbitality
{
    public class NeutralPlanetBuilder : BehaviourBuilder
    {
        public override PlanetAI CreateBehaviour(GameObject entity)
        {
            PlanetAI ai = entity.AddComponent<PlanetAI>();

            entity.layer = LayerMask.NameToLayer("Default");
            entity.tag = "Untagged";

            return ai;
        }
    }
}
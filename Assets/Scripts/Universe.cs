using UnityEngine;

/*
    Helper class for getting/caching all of the objects in the universe...
*/

namespace Orbitality
{
    public static class Universe
    {
        public static PhysicalObject[] allPhysicalObjects
        {
            get
            {
                if (_allPhysicalObjects == null)
                    _allPhysicalObjects = GameObject.FindObjectsOfType<PhysicalObject>();

                return _allPhysicalObjects;
            }
        }
        private static PhysicalObject[] _allPhysicalObjects;
    }
}
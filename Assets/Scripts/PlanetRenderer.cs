using UnityEngine;

/*
    Simple sprite renderer wrapper
    Visual ID passed with sprite to store data in case of game save
*/

namespace Orbitality
{
    public class PlanetRenderer : MonoBehaviour
    {
        public int VisualID
        {
            get { return _visualID; }
        }
        private int _visualID;

        [SerializeField] private SpriteRenderer body;

        public void SetMainBody(Sprite sprite, int id)
        {
            body.sprite = sprite;
            _visualID = id;
        }
    }
}
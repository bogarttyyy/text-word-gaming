using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class LetterPosition
    {
        public Vector2 position;
        public bool isOccupied = false;

        public LetterPosition(Vector3 position)
        {
            this.position = position;
        }
    }
}
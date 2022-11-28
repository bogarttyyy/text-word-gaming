using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class LetterPosition
    {
        public Vector2 position;
        public bool isOccupied = false;
        public LetterObj letterObj;

        public LetterPosition(Vector3 pos)
        {
            position = pos;
        }

        internal void SetObject(LetterObj obj)
        {
            letterObj = obj;
            letterObj.SetPosition(position);
            isOccupied = true;
        }

        internal void ClearObject()
        {
            letterObj?.ResetPosition();
            letterObj = null;
            isOccupied = false;
        }
    }
}
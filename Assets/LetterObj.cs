using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterObj : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.Question;
    public Vector3 displayPosition;
    public bool isTyped;

    private void Start()
    {
        displayPosition = transform.position;
        isTyped = false;
    }

    public char GetChar()
    {
        return $"{keyCode}"[0];
    }
}

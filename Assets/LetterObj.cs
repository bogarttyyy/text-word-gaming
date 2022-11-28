using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterObj : MonoBehaviour
{
    public KeyCode keyCode = KeyCode.Question;
    public Vector3 displayPosition;

    [SerializeField]
    private Vector3 typedPosition = Vector3.zero;
    public bool isTyped;

    private void Start()
    {
        displayPosition = transform.position;
        isTyped = false;
    }

    private void Update()
    {
        if (typedPosition != Vector3.zero && displayPosition != typedPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, typedPosition, 0.5f);
        }
    }

    public char GetChar()
    {
        return $"{keyCode}".ToLower()[0];
    }

    public void SetPosition(Vector3 position)
    {
        typedPosition = position;
    }
}

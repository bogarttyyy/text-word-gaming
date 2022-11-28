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
        displayPosition = transform.localPosition;
        isTyped = false;
    }

    private void Update()
    {
        if (typedPosition != Vector3.zero && displayPosition != typedPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, typedPosition, 0.2f);
        }

        if (typedPosition == Vector3.zero)
        {
            transform.localPosition = displayPosition;
        }
    }

    public char GetChar()
    {
        return $"{keyCode}".ToLower()[0];
    }

    public void SetPosition(Vector3 position)
    {
        isTyped = true;
        typedPosition = position;
    }

    public void ResetPosition()
    {
        isTyped = false;
        typedPosition = Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction touchPressAction;
    private InputAction touchPositionAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchLetter"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressPerformed;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressPerformed;
    }

    private void TouchPressPerformed(InputAction.CallbackContext context)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z = 0f;

        Debug.Log($"TouchPos: {position}");

        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null)
        {
            EventsManager.LetterTouchedEvent(hit.collider.GetComponent<LetterObj>());
            Debug.Log($"HitPos: {hit.transform}");
        }
    }

}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _input;

    public static bool MovementEnabled = true;

    public static event Action<Vector2> OnMovementPerformed;
    public static event Action<Vector2> OnLookPerformed;
    public static event Action OnEscapePerformed;

    public static event Action<PuzzleObject> OnPuzzleObjectSelected;
    public static event Action<EndPuzzle, int> OnEndPuzzleObjectSelected;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Gameplay.Movement.performed += Movement_performed;
        _input.Gameplay.Look.performed += Look_performed;
        _input.Gameplay.Interact.started += Interact_performed;
        _input.Gameplay.Escape.started += Escape_started;

        StaticEventsHandler.OnPuzzleCompleted += StaticEventsHandler_OnPuzzleCompleted;
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Gameplay.Movement.performed -= Movement_performed;
        _input.Gameplay.Look.performed -= Look_performed;
        _input.Gameplay.Interact.started -= Interact_performed;
        _input.Gameplay.Escape.started -= Escape_started;

        StaticEventsHandler.OnPuzzleCompleted -= StaticEventsHandler_OnPuzzleCompleted;
    }

    public static void CallEscapeButtonPressed()
    {
        if (!MovementEnabled)
        {
            EnableInput();
        }
        else
        {
            DisableInput();
        }

        OnEscapePerformed?.Invoke();
    }

    private void Start()
    {
        EnableInput();
    }

    private void Movement_performed(InputAction.CallbackContext ctx)
    {
        if (!MovementEnabled)
        {
            return;
        }

        OnMovementPerformed?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void Look_performed(InputAction.CallbackContext ctx)
    {
        if (!MovementEnabled)
        {
            return;
        }

        OnLookPerformed?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void Interact_performed(InputAction.CallbackContext ctx)
    {
        var control = ctx.control.name; // Get the name of the key pressed

        if (control == "e")
        {
            if (TryInteractWithPuzzleObject())
            {
                DisableInput();
            }
        }
        else if (control == "1" || control == "2" || control == "3" || control == "4")
        {
            TryInteractWithEndPuzzle(int.Parse(control));
        }
    }

    private void TryInteractWithEndPuzzle(int number)
    {
        var playerCamera = Camera.main.transform;

        var ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out var hit, 2f)) // 2f - interaction raycast range
        {
            if (hit.transform.TryGetComponent(out EndPuzzle endPuzzleObj))
            {
                OnEndPuzzleObjectSelected?.Invoke(endPuzzleObj, number);
            }
        }
    }

    private void Escape_started(InputAction.CallbackContext obj)
    {
        CallEscapeButtonPressed();
    }

    private void StaticEventsHandler_OnPuzzleCompleted(View obj)
    {
        EnableInput();
    }

    private bool TryInteractWithPuzzleObject()
    {
        var playerCamera = Camera.main.transform;

        var ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out var hit, 2f)) // 2f - interaction raycast range
        {
            if (hit.transform.TryGetComponent(out PuzzleObject puzzleObject))
            {
                OnPuzzleObjectSelected?.Invoke(puzzleObject);
                return true;
            }
        }

        return false;
    }

    private static void EnableInput()
    {
        MovementEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private static void DisableInput()
    {
        MovementEnabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}

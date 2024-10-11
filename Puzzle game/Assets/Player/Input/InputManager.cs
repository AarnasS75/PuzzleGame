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
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Gameplay.Movement.performed -= Movement_performed;
        _input.Gameplay.Look.performed -= Look_performed;
        _input.Gameplay.Interact.started -= Interact_performed;
        _input.Gameplay.Escape.started -= Escape_started;
    }

    private void Start()
    {
        HideCursor();
    }

    private void Movement_performed(InputAction.CallbackContext ctx)
    {
        var input = ctx.ReadValue<Vector2>();

        if (!MovementEnabled)
        {
            return;
        }

        OnMovementPerformed?.Invoke(input);
    }

    private void Look_performed(InputAction.CallbackContext ctx)
    {
        var input = ctx.ReadValue<Vector2>();

        if (!MovementEnabled)
        {
            return;
        }

        OnLookPerformed?.Invoke(input);
    }

    private void Interact_performed(InputAction.CallbackContext ctx)
    {
        if (TryInteractWithPuzzleObject())
        {
            MovementEnabled = false;
            ShowCursor();
        }
    }

    private void Escape_started(InputAction.CallbackContext obj)
    {
        if (!MovementEnabled)
        {
            MovementEnabled = true;
            HideCursor();
        }
        else
        {
            MovementEnabled = false;
            ShowCursor();
        }

        OnEscapePerformed?.Invoke();
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

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}

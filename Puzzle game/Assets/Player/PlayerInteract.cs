using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private float _interactionRange = 2f;

    public static event Action<PlayerInteract> OnPlayerInteract;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        var playerCamera = Camera.main.transform;

        var ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out var hit, _interactionRange, _interactionLayer))
        {
            print($"Interacted with: {hit.transform.name}");
            OnPlayerInteract?.Invoke(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Transform playerCamera = Camera.main.transform;
        if (playerCamera != null)
        {
            Gizmos.DrawRay(playerCamera.position, playerCamera.forward * _interactionRange);
        }
    }
}

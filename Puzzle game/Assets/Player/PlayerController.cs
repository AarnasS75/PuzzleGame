using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerMovement), typeof(PlayerInteract))]
public class PlayerController : MonoBehaviour
{
    public PlayerMovement Movement { get; private set; }
    public PlayerInteract Interaction { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        Interaction = GetComponent<PlayerInteract>();
    }
}

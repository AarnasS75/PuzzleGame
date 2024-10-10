using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundDrag;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckDistance = 0.4f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    public bool _isGrounded;

    [SerializeField] private Transform orientation;

    private Rigidbody _rb;

    private float horizontalInput;
    private float verticalInput;

    private float walkSpeed;
    private float sprintSpeed;
    private Vector3 moveDirection;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    private void Update()
    {
        // ground check
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckDistance, _whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (_isGrounded)
            _rb.drag = _groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (_isGrounded)
            _rb.AddForce(moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }

    private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckDistance);
        }
    }
}
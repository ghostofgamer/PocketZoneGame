using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private const string Speed = "Speed";
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField]private Joystick _joystick;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private Animator _animator;
    private float _flipValue = 180;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_moveInput.x < 0)
            transform.rotation = Quaternion.Euler(0, _flipValue, 0);
        else if (_moveInput.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        _animator.SetFloat(Speed, _moveInput.magnitude);
        // _moveInput = new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
        _moveInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);
    }
}
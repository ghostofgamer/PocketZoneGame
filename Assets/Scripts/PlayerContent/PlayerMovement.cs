using UnityEngine;

namespace PlayerContent
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerAnimation))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        
        private PlayerAnimation _playerAnimation;
        private Rigidbody2D _rb;
        private float _flipValue = 180;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerAnimation = GetComponent<PlayerAnimation>();
        }

        public void Move(Vector2 moveInput)
        {
            transform.rotation = moveInput.x switch
            {
                < 0 => Quaternion.Euler(0, _flipValue, 0),
                > 0 => Quaternion.Euler(0, 0, 0),
                _ => transform.rotation
            };

            _playerAnimation.PlayMovementSpeed(moveInput.magnitude);
            _rb.MovePosition(_rb.position + moveInput * _moveSpeed * Time.fixedDeltaTime);
        }
    }
}
using UnityEngine;

namespace PlayerContent
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        
        private PlayerMovement _playerMovement;
        private Vector2 _moveInput;

        private void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            _moveInput = new Vector2(_joystick.Horizontal, _joystick.Vertical);
            _playerMovement.Move(_moveInput);
        }
    }
}
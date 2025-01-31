using UnityEngine;

namespace PlayerContent
{
    public class PlayerAnimation : MonoBehaviour
    {
        private const string Speed = "Speed";
        private const string IndexWeapon = "IndexWeapon";
        
        [SerializeField] private Animator _animator;

        public void SetIndexWeapon(int index)
        {
            _animator.SetInteger(IndexWeapon, index);
        }

        public void PlayMovementSpeed(float movementSpeed)
        {
            _animator.SetFloat(Speed, movementSpeed);
        }
    }
}
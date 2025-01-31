using UnityEngine;

namespace CameraContent
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothSpeed = 0.125f;
        [SerializeField] private Vector3 _offset;

        private Vector3 _desiredPosition;
        private Vector3 _smoothedPosition;

        private void Update()
        {
            if (_target != null)
            {
                _desiredPosition = _target.position + _offset;
                _smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, _smoothSpeed);
                transform.position = _smoothedPosition;
                transform.LookAt(_target);
            }
        }
    }
}
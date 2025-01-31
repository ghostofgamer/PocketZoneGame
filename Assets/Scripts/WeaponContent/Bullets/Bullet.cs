using UnityEngine;

namespace WeaponContent.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void Update()
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        public void Init(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
using UnityEngine;

namespace WeaponContent
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _shotPosition;
        [SerializeField] private BulletContainer _bulletContainer;

        public void Shot()
        {
            _bulletContainer.GetBullet(_shotPosition);
        }
    }
}
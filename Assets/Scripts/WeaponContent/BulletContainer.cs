using SpawnerContent;
using UnityEngine;
using WeaponContent.Bullets;

namespace WeaponContent
{
    public class BulletContainer : MonoBehaviour
    {
        [SerializeField] private int _maxSize;
        [SerializeField] private Bullet _bullet;

        private ObjectPool<Bullet> _objectPool;

        private void Start()
        {
            _objectPool = new ObjectPool<Bullet>(_bullet, _maxSize, transform);
            _objectPool.EnableAutoExpand();
        }

        public void GetBullet(Transform shotPosition)
        {
            if (_objectPool.TryGetObject(out Bullet bullet, _bullet))
            {
                bullet.gameObject.SetActive(true);
                bullet.Init(shotPosition.position,shotPosition.rotation);
            }
        }
    }
}
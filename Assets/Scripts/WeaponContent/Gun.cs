using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _shotPosition;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shot();
        }
    }

    public void Shot()
    {
         Instantiate(_bullet, _shotPosition.position, _shotPosition.rotation, _container);
    }
}
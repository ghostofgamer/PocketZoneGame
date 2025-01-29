using System.Collections;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _shotPosition;
    
    private void Start()
    {
       StartCoroutine(Shot()); 
    }

    private IEnumerator Shot()
    {
        while (true)
        {
              yield return new WaitForSeconds(1f);
              Instantiate(_bullet, _shotPosition.position, _shotPosition.rotation, _container);
        }
    }
}

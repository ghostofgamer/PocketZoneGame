using UI.Buttons;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private Gun[] _guns;
    [SerializeField]private PlayerShot _playerShot;
    [SerializeField]private Animator _animator;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private int _currentGunIndex = 0;

    private void Start()
    {
        OffGuns();
        _playerAnimation.SetIndexWeapon(_currentGunIndex);
        _guns[_currentGunIndex].gameObject.SetActive(true);
        // _animator.SetInteger("IndexWeapon", _currentGunIndex);
        
        _playerShot.SetGun(_guns[_currentGunIndex]);
    }

    public void Change()
    {
        OffGuns();
        _currentGunIndex = (_currentGunIndex + 1) % _guns.Length;
        _playerAnimation.SetIndexWeapon(_currentGunIndex);
        _guns[_currentGunIndex].gameObject.SetActive(true);
        _playerShot.SetGun(_guns[_currentGunIndex]);
    }

    private void OffGuns()
    {
        foreach (var gun in _guns)
            gun.gameObject.SetActive(false);
    }
}

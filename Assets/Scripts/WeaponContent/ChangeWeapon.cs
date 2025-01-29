using UI.Buttons;
using UnityEngine;

public class ChangeWeapon : AbstractButton
{
    [SerializeField] private Gun[] _guns;
    [SerializeField]private PlayerShot _playerShot;

    private int _currentGunIndex = 0;
    
    private void Start()
    {
        OffGuns();
        _guns[_currentGunIndex].gameObject.SetActive(true);
        _playerShot.SetGun(_guns[_currentGunIndex]);
    }

    protected override void OnClick()
    {
        Change();
    }

    private void Change()
    {
        OffGuns();
        _currentGunIndex = (_currentGunIndex + 1) % _guns.Length;
        _guns[_currentGunIndex].gameObject.SetActive(true);
        _playerShot.SetGun(_guns[_currentGunIndex]);
    }

    private void OffGuns()
    {
        foreach (var gun in _guns)
            gun.gameObject.SetActive(false);
    }
}

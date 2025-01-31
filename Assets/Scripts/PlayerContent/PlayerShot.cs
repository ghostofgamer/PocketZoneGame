using UnityEngine;
using WeaponContent;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    
    private Gun _currentGun;

    public void SetGun(Gun gun)
    {
        _currentGun = gun;
    }
    
    public void Shot()
    {
        if (_inventory.Bullets <= 0) return;
        
        _currentGun.Shot();
        _inventory.DecreaseBullets();
    }
}
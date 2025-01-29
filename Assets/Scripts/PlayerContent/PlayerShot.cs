using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    private Gun _currentGun;

    public void SetGun(Gun gun)
    {
        _currentGun = gun;
    }
    
    public void Shot()
    {
        _currentGun.Shot();
    }
}
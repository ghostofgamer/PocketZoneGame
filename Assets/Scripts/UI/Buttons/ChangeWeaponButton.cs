using UI.Buttons;
using UnityEngine;

public class ChangeWeaponButton : AbstractButton
{
    [SerializeField] private ChangeWeapon _changeWeapon; 
    
    protected override void OnClick()
    {
      _changeWeapon.Change();
    }
}

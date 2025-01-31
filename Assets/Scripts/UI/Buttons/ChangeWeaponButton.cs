using UI.Buttons;
using UnityEngine;
using WeaponContent;

public class ChangeWeaponButton : AbstractButton
{
    [SerializeField] private ChangeWeapon _changeWeapon; 
    
    protected override void OnClick()
    {
      _changeWeapon.Change();
    }
}

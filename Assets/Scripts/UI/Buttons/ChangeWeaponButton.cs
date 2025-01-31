using UnityEngine;
using WeaponContent;

namespace UI.Buttons
{
    public class ChangeWeaponButton : AbstractButton
    {
        [SerializeField] private ChangeWeapon _changeWeapon;

        protected override void OnClick() => _changeWeapon.Change();
    }
}
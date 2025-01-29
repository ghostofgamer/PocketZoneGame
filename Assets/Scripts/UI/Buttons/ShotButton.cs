using UnityEngine;

namespace UI.Buttons
{
    public class ShotButton : AbstractButton
    {
        [SerializeField]private PlayerShot _playerShot;
    
        protected override void OnClick()
        {
            _playerShot.Shot();
        }
    }
}

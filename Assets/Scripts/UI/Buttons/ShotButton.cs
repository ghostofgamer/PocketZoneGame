using JetBrains.Annotations;
using UnityEngine;

namespace UI.Buttons
{
    public class ShotButton : AbstractButton
    {
        [SerializeField] [CanBeNull] private PlayerShot _playerShot;
    
        protected override void OnClick()
        {
            _playerShot.Shot();
        }
    }
}

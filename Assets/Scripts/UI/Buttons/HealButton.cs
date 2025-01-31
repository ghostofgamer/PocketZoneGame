using InventoryContent;
using PlayerContent;
using UnityEngine;

namespace UI.Buttons
{
    public class HealButton : AbstractButton
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private Inventory _inventory;

        protected override void OnClick()
        {
            if (_inventory.CheckHeal())
            {
                _inventory.UseMedicine();
                _playerHealth.Healing();
            }
        }
    }
}
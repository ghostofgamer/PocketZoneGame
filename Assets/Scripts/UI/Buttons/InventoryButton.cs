using InventoryContent;
using UnityEngine;

namespace UI.Buttons
{
    public class InventoryButton : AbstractButton
    {
        [SerializeField] private Inventory _inventory;

        protected override void OnClick() => _inventory.ChangeActivatedInventory();
    }
}
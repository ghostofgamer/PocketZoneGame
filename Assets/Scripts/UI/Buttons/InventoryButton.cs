using UI.Buttons;
using UnityEngine;

public class InventoryButton : AbstractButton
{
    [SerializeField] private Inventory _inventory;
    
    protected override void OnClick()
    {
        _inventory.ChangeActivatedInventory();
    }
}
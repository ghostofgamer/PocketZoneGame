using UI.Buttons;
using UnityEngine;

public class DeleteButton : AbstractButton
{
    [SerializeField]private ItemDrag _itemDrag;
    
    protected override void OnClick()
    {
        _itemDrag.Inventory.DeleteItem(_itemDrag.ID);
        gameObject.SetActive(false);
    }
}
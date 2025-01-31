using InventoryContent;
using UnityEngine;

namespace UI.Buttons
{
    public class DeleteButton : AbstractButton
    {
        [SerializeField]private ItemDrag _itemDrag;
    
        protected override void OnClick()
        {
            _itemDrag.Inventory.DeleteItem(_itemDrag.ID);
            gameObject.SetActive(false);
        }
    }
}
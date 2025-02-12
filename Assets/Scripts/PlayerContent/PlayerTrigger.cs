using InventoryContent;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerTrigger : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ItemPickUp item))
                _inventory.Add(item, item.Index, item.Count);
        }
    }
}
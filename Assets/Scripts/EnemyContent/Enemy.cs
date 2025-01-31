using InventoryContent;
using UnityEngine;

namespace EnemyContent
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform _target;
    
        public ItemPickUp ItemPickUp { get;private set; }
    
        public void SetItem(ItemPickUp itemPickUp)
        {
            ItemPickUp = itemPickUp;
        }
    }
}
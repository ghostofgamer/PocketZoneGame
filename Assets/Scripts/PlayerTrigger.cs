using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ItemPickUp item))
        {
            _inventory.Add(item,item.Index,item.Count);
        }
    }
}

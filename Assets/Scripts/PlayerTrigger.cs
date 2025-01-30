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
            Debug.Log("PlayerTrigger");
            _inventory.Add(item.Index,item.Count);
            item.gameObject.SetActive(false);               
        }
    }
}

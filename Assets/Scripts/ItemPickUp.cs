using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private int _count;
    
    public int Index => _index;
    public int Count => _count;
}

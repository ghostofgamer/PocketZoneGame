using System.Collections.Generic;
using UnityEngine;

namespace InventoryContent
{
    public class DataBase : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _sprites = new List<Sprite>();
        
        public List<Item> items = new List<Item>();
        
        public List<Sprite> Sprites => _sprites;
    }

    [System.Serializable]
    public class Item
    {
        public int id;
        public string name;
        public Sprite img;
        public bool isBullets;
        public bool isMedicine;
    }
}
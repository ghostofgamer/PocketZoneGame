using System;
using System.Collections;
using System.Collections.Generic;
using SaveDataContent;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryContent
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Storage _storage;
        [SerializeField] private DataBase _data;
        [SerializeField] private GameObject _gameObjShow;
        [SerializeField] private GameObject _inventoryMainObject;
        [SerializeField] private int _maxCount;
        [SerializeField] private Camera _cam;
        [SerializeField] private int _currentID;
        [SerializeField] private RectTransform _movingObject;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private GameObject _backGround;
         
        private List<ItemInventory> _items = new List<ItemInventory>();
        private ItemInventory _currentItem;
        private int _medicineCount;
        private bool _isDragging = false;
        private Vector2 _mouseDownPosition;
        private float _dragThreshold = 10f;
        private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);
        private int _maxStackSize = 130;
        
        public event Action<int> BulletsValueChanged;
        
        public event Action InventoryChanged;
        
        public int Bullets { get; private set; }
        
        public List<ItemInventory> Items => _items;

        private void Start()
        {
            if (_items.Count == 0)
                AddGraphics();
        
            SaveData save = _storage.LoadDataInfo();

            if (save != null)
            {
                foreach (var item in save.items)
                {
                    if (item.id > 0)
                        AddItem(item.idPosition,_data.items[item.id],item.count);
                }
            }
            
            UpdateInventory();
            CheckBullets();
        }

        public void Add(ItemPickUp itemPickUp, int id, int count)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].id == 0)
                {
                    AddItem(i, _data.items[id], count);
                    itemPickUp.gameObject.SetActive(false);  
                
                    if (_data.items[id].isBullets)
                        BulletsStartCheck();
                
                    return;
                }
            }
        }

        public void Update()
        {
            if (_currentID != -1)
                MoveObject();
        }

        public void ChangeActivatedInventory()
        {
            _backGround.SetActive(!_backGround.activeSelf);

            if (_backGround.activeSelf)
                UpdateInventory();
        }
    
        private void AddItem(int id, Item item, int count)
        {
            _items[id].id = item.id;
            _items[id].count = count;
            _items[id].itemGameObject.GetComponent<Image>().sprite = item.img;

            if (count > 1 && item.id != 0)
                _items[id].itemGameObject.GetComponentInChildren<Text>().text = count.ToString();
            else
                _items[id].itemGameObject.GetComponentInChildren<Text>().text = "";

            UpdateInventory();
            InventoryChanged?.Invoke();
        }

        public void AddInventoryItem(int id, ItemInventory invItem)
        {
            _items[id].id = invItem.id;
            _items[id].count = invItem.count;
            _items[id].itemGameObject.GetComponent<Image>().sprite = _data.items[invItem.id].img;

            if (invItem.count > 1 && invItem.id != 0)
                _items[id].itemGameObject.GetComponentInChildren<Text>().text = invItem.count.ToString();
            else
                _items[id].itemGameObject.GetComponentInChildren<Text>().text = "";
        }

        private void AddGraphics()
        {
            for (int i = 0; i < _maxCount; i++)
            {
                GameObject newItem = Instantiate(_gameObjShow, _inventoryMainObject.transform) as GameObject;
                newItem.GetComponent<ItemDrag>().Init(this, i);
                newItem.name = i.ToString();
                ItemInventory ii = new ItemInventory();
                ii.itemGameObject = newItem;
                RectTransform rt = newItem.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(0, 0, 0);
                rt.localScale = new Vector3(1, 1, 1);
                newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);
                Button tempButton = newItem.GetComponent<Button>();
                _items.Add(ii);
            }
        }

        private void UpdateInventory()
        {
            for (int i = 0; i < _maxCount; i++)
            {
                if (_items[i].id != 0 && _items[i].count > 1)
                    _items[i].itemGameObject.GetComponentInChildren<Text>().text = _items[i].count.ToString();
                else
                    _items[i].itemGameObject.GetComponentInChildren<Text>().text = "";

                _items[i].itemGameObject.GetComponent<Image>().sprite = _data.items[_items[i].id].img;
                _items[i].isBullets = _data.items[_items[i].id].isBullets;
                _items[i].isMedicine = _data.items[_items[i].id].isMedicine;
            }
            
            InventoryChanged?.Invoke();
        }
        
        private void MoveObject()
        {
            Vector3 pos = Input.mousePosition + _offset;
            pos.z = _inventoryMainObject.GetComponent<RectTransform>().position.z;
            _movingObject.position = _cam.ScreenToWorldPoint(pos);
        }

        private ItemInventory CopyInventoryItem(ItemInventory old)
        {
            ItemInventory New = new ItemInventory();
            New.id = old.id;
            New.itemGameObject = old.itemGameObject;
            New.count = old.count;
            return New;
        }

        private void BulletsStartCheck()
        {
            StartCoroutine(BulletsCheck());
        }

        private IEnumerator BulletsCheck()
        {
            yield return _waitForSeconds;
            CheckBullets();
        }

        private void CheckBullets()
        {
            Bullets = 0;

            foreach (var item in _items)
            {
                if (item.isBullets)
                    Bullets += item.count;
            }
        
            BulletsValueChanged?.Invoke(Bullets);
        }

        public bool CheckHeal()
        {
            _medicineCount = 0;
        
            foreach (var item in _items)
            {
                if (item.isMedicine)
                    _medicineCount += item.count;
            }

            return _medicineCount > 0;
        }

        public void UseMedicine()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].isMedicine)
                {
                    _items[i].count -= 1;
                    _items[i].itemGameObject.GetComponentInChildren<Text>().text =   _items[i].count.ToString();
                    _medicineCount--;
                    
                    if (_items[i].count <= 0)
                        AddItem(i, _data.items[0], 0);

                    UpdateInventory();
                    return;
                }
            }
        }

        public void DecreaseBullets()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].isBullets)
                {
                    _items[i].count -= 1;
                    _items[i].itemGameObject.GetComponentInChildren<Text>().text =   _items[i].count.ToString();
                    Bullets--;
                    BulletsValueChanged?.Invoke(Bullets);
                    
                    if (_items[i].count <= 0)
                        AddItem(i, _data.items[0], 0);
                    
                    UpdateInventory();
                    return;
                }
            }
        }

        public void DeleteItem(int id)
        {
            AddItem(id, _data.items[0], 0);
            CheckBullets();
            UpdateInventory();
        }


        public void OnItemDragStart(int id)
        {
            if (_items[id].id == 0) return;

            _currentID = id;
            _currentItem = CopyInventoryItem(_items[_currentID]);
            _movingObject.gameObject.SetActive(true);
            _movingObject.GetComponent<Image>().sprite = _data.Sprites[_currentItem.id];
            AddItem(_currentID, _data.items[0], 0);
            _isDragging = true;
        }

        public void OnItemDragEnd(int id)
        {
            if (_currentID != -1)
            {
                ItemInventory II = _items[id];

                if (_currentItem.id != II.id)
                {
                    AddInventoryItem(_currentID, II);
                    AddInventoryItem(id, _currentItem);
                }
                else
                {
                    if (II.count + _currentItem.count <= _maxStackSize)
                    {
                        II.count += _currentItem.count;
                    }
                    else
                    {
                        AddItem(_currentID, _data.items[II.id], II.count + _currentItem.count - _maxStackSize);
                        II.count = _maxStackSize;
                    }

                    II.itemGameObject.GetComponentInChildren<Text>().text = II.count.ToString();
                }

                _currentID = -1;
                _movingObject.gameObject.SetActive(false);
                _isDragging = false;
                
                InventoryChanged?.Invoke();
            }
        }

        public void OnItemClick(int id)
        {
            if (_items[id].id == 0)
            {
                Debug.Log(_items[id].id);
                return;
            }
        }

        public bool CheckEmpty(int id)
        {
            return _items[id].id > 0;
        }
    }

    [System.Serializable]
    public class ItemInventory
    {
        public int id;
        public GameObject itemGameObject;
        public int count;
        public bool isBullets;
        public bool isMedicine;
    }
}
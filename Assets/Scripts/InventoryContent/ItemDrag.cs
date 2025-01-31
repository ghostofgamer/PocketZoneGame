using System;
using InventoryContent;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private int _id;
    [SerializeField] private GameObject _closeButton;

    private Inventory _inventory;
    private bool _isCheck;
    private float _currentTime = 0;
    private float _dragThreshold = 0.15f;
    private bool _isDragging = false;

    public int ID => _id;

    public Inventory Inventory => _inventory;

    public void Init(Inventory inventory, int id)
    {
        _inventory = inventory;
        _id = id;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _currentTime = 0;
        _isCheck = true;
        _isDragging = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isCheck = false;

        if (_isDragging)
        {
            GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;

            if (targetObject != null)
            {
                if (targetObject.TryGetComponent<ItemDrag>(out ItemDrag itemDrag))
                    _inventory.OnItemDragEnd(itemDrag.ID);
            }

            _inventory.OnItemDragEnd(_id);
        }
        else
        {
            if (_inventory.CheckEmpty(_id))
                _closeButton.SetActive(!_closeButton.activeSelf);

            _inventory.OnItemClick(_id);
        }

        _currentTime = 0;
        _isDragging = false;
    }

    private void Update()
    {
        if (_isCheck && !_isDragging)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _dragThreshold)
            {
                Debug.Log("Dragging");
                _closeButton.SetActive(false);
                _inventory.OnItemDragStart(_id);
                _isDragging = true;
            }
        }
    }
}
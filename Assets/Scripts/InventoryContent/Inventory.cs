using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    public DataBase data;
    public List<ItemInventory> items = new List<ItemInventory>();
    public GameObject gameObjShow;
    public GameObject InventoryMainObject;
    public int maxCount;

    public Camera cam;
    public EventSystem es;
    public int currentID;
    public ItemInventory currentItem;
    public RectTransform movingObject;
    public Vector3 offset;
    public GameObject backGround;

    public event Action<int> BulletsValueChanged;

    public int Bullets { get; private set; }
    
    private bool isDragging = false;
    private Vector2 mouseDownPosition;
    private float dragThreshold = 10f;

    private void Start()
    {
        if (items.Count == 0)
        {
            AddGraphics();
        }

        /*
        for (int i = 0; i < maxCount; i++)
        {
            // AddItem(i, data.items[0], 0);

            AddItem(i, data.items[Random.Range(0, data.items.Count)], Random.Range(0, 99));
        }
        */

        UpdateInventory();
        CheckBullets();
    }

    public void Add(int id, int count)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == 0)
            {
                AddItem(i, data.items[id], count);

                if (data.items[id].isBullets)
                {
                    Debug.Log("Adding Bullet");
                    BulletsStartCheck();
                }


                return;
            }
        }

        Debug.Log("нет места ");
    }

    public void Update()
    {
        if (currentID != -1)
        {
            MoveObject();
        }
        else
        {
            UpdateInventory();
        }
    }

    public void ChangeActivatedInventory()
    {
        backGround.SetActive(!backGround.activeSelf);

        if (backGround.activeSelf)
            UpdateInventory();
    }

    /*public void SearchForSameItem(Item item, int count)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id == item.id)
            {
                if (items[i].count < 128)
                {
                    items[i].count += count;

                    if (items[i].count > 128)
                    {
                        count = items[i].count - 128;
                        items[i].count = 64;
                    }
                    else
                    {
                        count = 0;
                        i = maxCount;
                    }
                }
            }
        }

        if (count > 0)
        {
            for (int i = 0; i < maxCount; i++)
            {
                if (items[i].id == 0)
                {
                    AddItem(i, item, count);
                    i = maxCount;
                }
            }
        }
    }*/

    public void AddItem(int id, Item item, int count)
    {
        items[id].id = item.id;
        items[id].count = count;
        items[id].itemGameObject.GetComponent<Image>().sprite = item.img;
        // items[id].itemGameObject.GetComponent<Image>().sprite = data.items[item.id].img;

        if (count > 1 && item.id != 0)
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddInventoryItem(int id, ItemInventory invItem)
    {
        items[id].id = invItem.id;
        items[id].count = invItem.count;
        items[id].itemGameObject.GetComponent<Image>().sprite = data.items[invItem.id].img;

        // items[id].isBullets = invItem.isBullets;

        if (invItem.count > 1 && invItem.id != 0)
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = invItem.count.ToString();
        }
        else
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddGraphics()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newItem = Instantiate(gameObjShow, InventoryMainObject.transform) as GameObject;
            newItem.GetComponent<ItemDrag>().Init(this,i);
            newItem.name = i.ToString();
            ItemInventory ii = new ItemInventory();
            ii.itemGameObject = newItem;
            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);

            Button tempButton = newItem.GetComponent<Button>();

            // tempButton.onClick.AddListener(delegate { SelectObject(); });

            items.Add(ii);
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id != 0 && items[i].count > 1)
            {
                items[i].itemGameObject.GetComponentInChildren<Text>().text = items[i].count.ToString();
            }
            else
            {
                items[i].itemGameObject.GetComponentInChildren<Text>().text = "";
            }

            items[i].itemGameObject.GetComponent<Image>().sprite = data.items[items[i].id].img;
            items[i].isBullets = data.items[items[i].id].isBullets;
        }
    }

    public void SelectObject()
    {
        if (currentID == -1)
        {
            /*if (data.items[currentItem.id].id == 0)
                return;*/

            currentID = int.Parse(es.currentSelectedGameObject.name);

            if (items[currentID].id == 0)
            {
                Debug.Log(items[currentID].id);
                currentID = -1;
                return;
            }
            
           // bool drag =  StartCheckDrag();
            

            currentItem = CopyInventoryItem(items[currentID]);
            movingObject.gameObject.SetActive(true);
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;
            AddItem(currentID, data.items[0], 0);
        }
        else
        {
            ItemInventory II = items[int.Parse(es.currentSelectedGameObject.name)];

            if (currentItem.id != II.id)
            {
                AddInventoryItem(currentID, II);
                AddInventoryItem(int.Parse(es.currentSelectedGameObject.name), currentItem);
            }
            else
            {
                if (II.count + currentItem.count <= 128)
                {
                    II.count += currentItem.count;
                }
                else
                {
                    AddItem(currentID, data.items[II.id], II.count + currentItem.count - 128);
                    II.count = 128;
                }

                II.itemGameObject.GetComponentInChildren<Text>().text = II.count.ToString();
            }

            currentID = -1;
            movingObject.gameObject.SetActive(false);
        }
    }

    public void MoveObject()
    {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);
    }

    public ItemInventory CopyInventoryItem(ItemInventory old)
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
        yield return new WaitForSeconds(0.1f);
        CheckBullets();
    }

    public void CheckBullets()
    {
        Bullets = 0;

        foreach (var item in items)
        {
            if (item.isBullets)
            {
                Bullets += item.count;
                // BulletsValueChanged?.Invoke(Bullets);
                Debug.Log("патроны " + Bullets);
            }
        }

        /*if (Bullets == 0)*/
            BulletsValueChanged?.Invoke(Bullets);
    }

    public void DecreaseBullets()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].isBullets)
            {
                items[i].count -= 1;
                // items[id].count = invItem.count;      
                if (items[i].count <= 0)
                    AddItem(i, data.items[0], 0);

                return;
            }
        }

        Bullets--;
        BulletsValueChanged?.Invoke(Bullets);
    }

    public void DeleteItem(int id )
    {
        AddItem(id, data.items[0], 0);
        CheckBullets();
        UpdateInventory();
    }
    
    
    
    public void OnItemDragStart(int id)
    {
        if (items[id].id == 0) return;
        
        currentID = id;
        currentItem = CopyInventoryItem(items[currentID]);
        movingObject.gameObject.SetActive(true);
        movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;
        AddItem(currentID, data.items[0], 0);
        isDragging = true;
    }

    public void OnItemDragEnd(int id)
    {
        if (currentID != -1)
        {
            ItemInventory II = items[id];
            Debug.Log("не -1 ");

            if (currentItem.id != II.id)
            {
                AddInventoryItem(currentID, II);
                AddInventoryItem(id, currentItem);
            }
            else
            {
                if (II.count + currentItem.count <= 128)
                {
                    II.count += currentItem.count;
                }
                else
                {
                    AddItem(currentID, data.items[II.id], II.count + currentItem.count - 128);
                    II.count = 128;
                }

                II.itemGameObject.GetComponentInChildren<Text>().text = II.count.ToString();
            }

            currentID = -1;
            movingObject.gameObject.SetActive(false);
            isDragging = false;
        }
    }

    public void OnItemClick(int id)
    {
        if (items[id].id == 0)
        {
            Debug.Log(items[id].id);
            return;
        }

        // Логика открытия окна или другой логики при клике на предмет
        Debug.Log("Item clicked: " + id);
    }

    public bool CheckEmpty(int id)
    {
        return items[id].id > 0;
    }
}

[System.Serializable]
public class ItemInventory
{
    public int id;
    public GameObject itemGameObject;
    public int count;
    public bool isBullets;
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using PlayerContent;
using SaveDataContent;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Inventory _inventory;

    private string filePath;

    private void OnEnable()
    {
        _playerHealth.HealthChanged += SavePlayerHealth;
    }

    private void OnDisable()
    {
        _playerHealth.HealthChanged -= SavePlayerHealth;
    }

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    /*public void SaveGame()
    {
        StartCoroutine(SaveDataInfo());
    }*/

    public void SaveInventory()
    {
        StartCoroutine(SaveInventoryData());
    }

    public void SavePlayerHealth()
    {
        StartCoroutine(SavePlayerHealthData());
    }


    public IEnumerator SavePlayerHealthData()
    {
        SaveData saveData = LoadDataInfo();
        
        if (saveData == null)
            saveData = new SaveData();
        
        saveData.health = _playerHealth.Health;

        Debug.Log("Health " + saveData.health);
        string jsonData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonData);
        yield return null;
    }

    public IEnumerator SaveInventoryData()
    {
        SaveData saveData = LoadDataInfo();

        if (saveData == null)
            saveData = new SaveData();

        saveData.items = new List<ItemData>(_inventory.items.Count);

        for (int i = 0; i < _inventory.items.Count; i++)
        {
            saveData.items.Add(new ItemData(_inventory.items[i].id, _inventory.items[i].count, i));
        }

        string jsonData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(filePath, jsonData);
        yield return null;
    }


    /*public IEnumerator SaveDataInfo()
    {
        SaveData saveData = new SaveData
        {
            items = new List<ItemData>(_inventory.items.Count)
        };

        for (int i = 0; i < _inventory.items.Count; i++)
        {
            saveData.items.Add(new ItemData(_inventory.items[i].id, _inventory.items[i].count, i));
        }

        // foreach (var item in saveData.items)
        // {
        //     Debug.Log(item.id + "  ,,,  "  + item.idPosition + "   ...     " + item.count  );
        // }

        saveData.health = _playerHealth.Health;

        Debug.Log("Health " +  saveData.health);
        string jsonData = JsonUtility.ToJson(saveData, true);
        // Debug.Log("Saving data: " + jsonData);
        File.WriteAllText(filePath, jsonData);
        yield return null;
    }*/

    public SaveData LoadDataInfo()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            Debug.Log("Loading data: " + jsonData);
            return JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            return null;
        }
    }

    public void ClearSaveData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save data file deleted.");
        }
        else
        {
            Debug.Log("Save data file does not exist.");
        }
    }
}
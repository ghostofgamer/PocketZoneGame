using System.Collections;
using System.Collections.Generic;
using System.IO;
using InventoryContent;
using PlayerContent;
using UnityEngine;

namespace SaveDataContent
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private Inventory _inventory;

        private string filePath;

        private void OnEnable()
        {
            _playerHealth.HealthChanged += SavePlayerHealth;
            _inventory.InventoryChanged += SaveInventory;
        }

        private void OnDisable()
        {
            _playerHealth.HealthChanged -= SavePlayerHealth;
            _inventory.InventoryChanged -= SaveInventory;
        }

        private void Start()
        {
            filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        }

        private void SaveInventory()
        {
            StartCoroutine(SaveInventoryData());
        }

        private void SavePlayerHealth()
        {
            StartCoroutine(SavePlayerHealthData());
        }
        
        private IEnumerator SavePlayerHealthData()
        {
            SaveData saveData = CreateSaveData();
            saveData.health = _playerHealth.Health;
            SaveData(saveData);
            yield return null;
        }

        private IEnumerator SaveInventoryData()
        {
            SaveData saveData = CreateSaveData();
            saveData.items = new List<ItemData>(_inventory.items.Count);

            for (int i = 0; i < _inventory.items.Count; i++)
                saveData.items.Add(new ItemData(_inventory.items[i].id, _inventory.items[i].count, i));

            SaveData(saveData);
            yield return null;
        }

        public SaveData LoadDataInfo()
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonUtility.FromJson<SaveData>(jsonData);
            }
            else
            {
                return null;
            }
        }

        private SaveData CreateSaveData()
        {
            SaveData saveData = LoadDataInfo() ?? new SaveData();
            return saveData;
        }
        
        private void SaveData(SaveData saveData)
        {
            string jsonData = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(filePath, jsonData);
        }
        
        public void ClearSaveData()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            else
                Debug.Log("Save data file does not exist.");
        }
    }
}
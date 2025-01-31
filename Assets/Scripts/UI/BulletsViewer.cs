using TMPro;
using UnityEngine;

public class BulletsViewer : MonoBehaviour
{
    [SerializeField] private TMP_Text _bulletText;
    [SerializeField]private Inventory _inventory;

    private void OnEnable()
    {
        _inventory.BulletsValueChanged += Show;
    }

    private void OnDisable()
    {
        _inventory.BulletsValueChanged -= Show;
    }

    public void Show(int value)
    {
        _bulletText.text = value.ToString();
    }
}
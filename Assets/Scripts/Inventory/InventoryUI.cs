using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private RectTransform slotsParent; // Використовуємо RectTransform для позиціонування
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject slotPrefab;

    [SerializeField] private int rows = 8; // Кількість рядків
    [SerializeField] private int columns = 12; // Кількість колонок
    [SerializeField] private float slotSpacing = 10f; // Відступ між слотами

    private GameObject[,] slots; // Двовимірний масив для збереження слотів

    private void Start()
    {
        GenerateSlots();
        PopulateSlots();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            bool openInventor = !inventoryUI.activeSelf;
            inventoryUI.SetActive(openInventor);

            Cursor.lockState = openInventor ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = openInventor;
        }
    }

    private void GenerateSlots()
    {
        slots = new GameObject[rows, columns];

        float slotWidth = slotPrefab.GetComponent<RectTransform>().sizeDelta.x + slotSpacing;
        float slotHeight = slotPrefab.GetComponent<RectTransform>().sizeDelta.y + slotSpacing;

        // Центруємо сітку відносно батьківського об'єкта
        Vector2 startPosition = new Vector2(
            -(columns * slotWidth) / 2 + slotWidth / 2,
            (rows * slotHeight) / 2 - slotHeight / 2
        );

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Розрахунок позиції кожного слота
                Vector2 slotPosition = startPosition + new Vector2(col * slotWidth, -row * slotHeight);

                // Створення слота
                GameObject slot = Instantiate(slotPrefab, slotsParent);
                slot.GetComponent<RectTransform>().anchoredPosition = slotPosition;
                slot.name = $"Slot_{row}_{col}";

                // Збереження слота у масив
                slots[row, col] = slot;

                // Вимикаємо іконку за замовчуванням
                Transform iconTransform = slot.transform.Find("Icon");
                if (iconTransform != null)
                {
                    iconTransform.GetComponent<Image>().enabled = false;
                }
            }
        }
    }

    private void PopulateSlots()
    {
        int index = 0;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject slot = slots[row, col];
                Transform iconTransform = slot.transform.Find("Icon");

                if (index < inventory.items.Count) // Якщо є предмет
                {
                    Items item = inventory.items[index];
                    if (iconTransform != null)
                    {
                        Image slotImage = iconTransform.GetComponent<Image>();
                        slotImage.sprite = item.itemIcon;
                        slotImage.enabled = true; // Увімкнути іконку
                    }
                    index++;
                }
                else if (iconTransform != null) // Якщо предмету немає
                {
                    iconTransform.GetComponent<Image>().enabled = false; // Вимкнути іконку
                }
            }
        }
    }
}

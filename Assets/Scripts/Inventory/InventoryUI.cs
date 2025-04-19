using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private RectTransform slotsParent;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Thoughs thoughs;
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private int rows = 8;
    [SerializeField] private int columns = 12;
    [SerializeField] private float slotSpacing = 10f;

    private GameObject[,] slots;
    [Header("List objects")]
    [SerializeField] private GameObject listSprite;
    [SerializeField] private GameObject listText;
    [SerializeField] public bool openInventor = false;
    [SerializeField] private GameObject numPanel;
    private bool listWatch = false;

    [Header("Active item elements")]
    [SerializeField] private GameObject AOpanel;

    [SerializeField] private GameController controller;

    [SerializeField] private Items[] itemSO;

    private void Awake()
    {
        GenerateSlots();
    }

    private void Start()
    {
        foreach(Items itemS in itemSO)
        {
            itemS.isTaskChanged = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q) && !listWatch && !openInventor && controller.canMove && !pauseMenu.isPause)
        {
            openInventor = !inventoryUI.activeSelf;
            inventoryUI.SetActive(openInventor);

            Cursor.lockState = openInventor ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = openInventor;
            controller.canMove = !openInventor;
        }

        if(openInventor && !pauseMenu.isPause && Input.GetKeyUp(KeyCode.Escape))
        {
            openInventor = false;
            inventoryUI.SetActive(openInventor);

            Cursor.lockState = openInventor ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = openInventor;
            controller.canMove = !openInventor;
        }

        if(inventory.activeItem != null)
        {
            if (inventory.activeItem.listText != "null")
            {
                if (Input.GetMouseButtonDown(0) && !openInventor && !numPanel.activeInHierarchy)
                {
                    listWatch = !listWatch;
                    ShowList();
                    if(inventory.activeItem.thoughIndex != 0)
                    {
                        thoughs.ShowThought(inventory.activeItem.thoughIndex);
                    }
                }
            }            
        }

        PopulateSlots();
    }


    private void GenerateSlots()
    {
        slots = new GameObject[rows, columns];

        float slotWidth = slotPrefab.GetComponent<RectTransform>().sizeDelta.x + slotSpacing;
        float slotHeight = slotPrefab.GetComponent<RectTransform>().sizeDelta.y + slotSpacing;

        Vector2 startPosition = new Vector2(
            -(columns * slotWidth) / 2 + slotWidth / 2,
            (rows * slotHeight) / 2 - slotHeight / 2
        );

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector2 slotPosition = startPosition + new Vector2(col * slotWidth, -row * slotHeight);

                GameObject slot = Instantiate(slotPrefab, slotsParent);
                slot.GetComponent<RectTransform>().anchoredPosition = slotPosition;
                slot.name = $"Slot_{row}_{col}";

                slots[row, col] = slot;

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

                if (index < inventory.items.Count)
                {
                    Items item = inventory.items[index];
                    if (iconTransform != null)
                    {
                        Image slotImage = iconTransform.GetComponent<Image>();
                        slotImage.sprite = item.itemIcon;
                        slotImage.enabled = true;
                        Button button = iconTransform.GetComponent<Button>();
                        if (button != null)
                        {
                            int capturedIndex = index;
                            button.onClick.RemoveAllListeners();
                            button.onClick.AddListener(() =>
                            {
                                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                                {
                                    Debug.Log($"Dropping item: {item.itemName}");
                                    AOpanel.GetComponent<Image>().sprite = null;
                                    AOpanel.SetActive(false);
                                    inventory.RemoveItem(item);
                                }
                                else
                                {
                                    AOpanel.SetActive(true);
                                    Debug.Log($"Selecting item: {item.itemName}");
                                    inventory.SelectItem(item);
                                    AOpanel.GetComponent<Image>().sprite = inventory.activeItem.itemIcon;
                                }

                            });
                        }
                    }
                    index++;
                }
                else if (iconTransform != null)
                {
                    iconTransform.GetComponent<Image>().enabled = false;
                }
            }
        }
    }

    private void ShowList()
    {
        listSprite.GetComponent<Image>().sprite = inventory.activeItem.itemIcon;
        listText.GetComponent<TextMeshProUGUI>().text = inventory.activeItem.listText;
        listSprite.SetActive(listWatch);
        listText.SetActive(listWatch);
    }

    public void CheckButtonWork()
    {
        Debug.Log("Button works");
    }
}

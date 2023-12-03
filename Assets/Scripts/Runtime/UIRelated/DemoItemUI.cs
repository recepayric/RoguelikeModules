using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Runtime;
using Runtime.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Item = Runtime.ItemsRelated.Item;
using Random = UnityEngine.Random;

public class DemoItemUI : MonoBehaviour
{
    public GameObject itemPanel;
    public ItemDataSo itemSo;

    // Start is called before the first frame update
    void Start()
    {
        itemButtons[0].onClick.AddListener(delegate { BuyItem(0); });
        itemButtons[1].onClick.AddListener(delegate { BuyItem(1); });
        itemButtons[2].onClick.AddListener(delegate { BuyItem(2); });
        itemButtons[3].onClick.AddListener(delegate { BuyItem(3); });
    }

    public GameObject[] itemPanels;

    public TextMeshProUGUI[] itemNames;
    public TextMeshProUGUI[] itemStats;
    public Button[] itemButtons;

    public Item[] itemsOnScreen;

    [Button]
    public void GetTextMeshes()
    {
        itemNames = new TextMeshProUGUI[itemPanels.Length];
        itemStats = new TextMeshProUGUI[itemPanels.Length];
        itemButtons = new Button[itemPanels.Length];
        itemsOnScreen = new Item[itemPanels.Length];

        for (int i = 0; i < itemPanels.Length; i++)
        {
            var nameText = itemPanels[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var statsText = itemPanels[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            var button = itemPanels[i].transform.GetChild(2).GetComponent<Button>();

            itemNames[i] = nameText;
            itemStats[i] = statsText;
            itemButtons[i] = button;
        }
    }

    public void GetRandomItems()
    {
        var totalItemCount = itemSo.itemData.Count;

        for (int i = 0; i < itemPanels.Length; i++)
        {
            itemPanels[i].SetActive(true);

            var randItem = itemSo.itemData[Random.Range(0, totalItemCount)];
            itemsOnScreen[i] = randItem;
            randItem.itemStats.SetStats();

            itemNames[i].text = randItem.name;
            itemStats[i].text = GetStatTexts(randItem);
        }
    }

    private string GetStatTexts(Item item)
    {
        var itemString = "";

        foreach (var stat in item.itemStats.stats)
        {
            itemString += stat.Key + ": " + stat.Value + "\n";
        }


        return itemString;
    }

    private void BuyItem(int index)
    {
        Debug.Log("Buying the item!");
        itemPanels[index].SetActive(false);

        itemsOnScreen[index].quantity++;
        ScriptDictionaryHolder.Player.AddItem(itemsOnScreen[index]);
    }


    private void OpenPanel()
    {
        itemPanel.SetActive(true);
        GetRandomItems();
    }

    private void ClosePanel()
    {
        itemPanel.SetActive(false);
    }
    
    private void OnFloorEnds()
    {
        OpenPanel();
    }

    private void OnFloorLoads()
    {
        ClosePanel();
    }

    private void OnEnable()
    {
        EventManager.Instance.FloorEndsEvent += OnFloorEnds;
        EventManager.Instance.FloorLoadEvent += OnFloorLoads;
    }

    private void OnDisable()
    {
        EventManager.Instance.FloorEndsEvent -= OnFloorEnds;
        EventManager.Instance.FloorLoadEvent -= OnFloorLoads;
    }
}
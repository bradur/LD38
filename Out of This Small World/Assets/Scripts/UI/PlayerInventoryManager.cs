// Date   : 22.04.2017 16:58
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInventoryManager : MonoBehaviour {

    [SerializeField]
    private PlayerInventoryItem playerInventoryItemPrefab;
    private List<PlayerInventoryItem> items = new List<PlayerInventoryItem>();

    private Vector2 initialPosition;
    private Vector2 itemSize;

    public void Gain(GenericObjectStruct genericObjectStruct)
    {
        PlayerInventoryItem newItem = Instantiate(playerInventoryItemPrefab);
        RectTransform rt = newItem.GetComponent<RectTransform>();
        itemSize = rt.sizeDelta;
        initialPosition = rt.anchoredPosition;
        newItem.transform.SetParent(transform, false);
        newItem.Init(genericObjectStruct);
        items.Add(newItem);
        UpdatePositions();
    }

    public bool UseKey(KeyColor color)
    {
        for (int i = 0; i < items.Count; i += 1)
        {

            PlayerInventoryItem inventoryItem = items[i];
            if (inventoryItem.GenericObjectStruct.keyColorType == color)
            {
                Use(inventoryItem);
                return true;
            }
        }
        return false;
    }

    public GenericObjectStruct GetKey(KeyColor color)
    {
        for (int i = 0; i < items.Count; i += 1)
        {
            
            PlayerInventoryItem inventoryItem = items[i];
            if (inventoryItem.GenericObjectStruct.keyColorType == color)
            {
                return inventoryItem.GenericObjectStruct;
            }
        }
        return null;
    }

    public PlayerInventoryItem GetItem(ObjectType objectType)
    {
        for (int i = 0; i < items.Count; i += 1)
        {
            PlayerInventoryItem inventoryItem = items[i];
            if (inventoryItem.GenericObjectStruct.objectType == objectType)
            {
                return inventoryItem;
            }
        }
        return null;
    }

    public void Use(PlayerInventoryItem item)
    {
        items.Remove(item);
        item.Kill();
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        int addY = 15;
        for (int i = 0; i < items.Count; i += 1)
        {
            PlayerInventoryItem inventoryItem = items[i];
            RectTransform rt = inventoryItem.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(initialPosition.x, -(i * itemSize.y + i * addY));
        }
    }

}

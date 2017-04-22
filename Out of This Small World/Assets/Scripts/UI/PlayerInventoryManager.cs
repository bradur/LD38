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

    public void Use(PlayerInventoryItem item)
    {
        items.Remove(item);
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

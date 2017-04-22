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

    public void Gain(GenericObjectStruct genericObjectStruct)
    {
        PlayerInventoryItem newItem = Instantiate(playerInventoryItemPrefab);
        newItem.transform.SetParent(transform, false);
        newItem.Init(genericObjectStruct);
        items.Add(newItem);
    }

}

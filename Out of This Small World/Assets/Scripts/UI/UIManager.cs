// Date   : 22.04.2017 18:21
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private PlayerInventoryManager playerInventoryManager;

    public void InventoryGain(GenericObjectStruct genericObjectStruct)
    {
        playerInventoryManager.Gain(genericObjectStruct);
    }

    [SerializeField]
    ToolTipManager toolTipManager;

    public void ShowToolTip(string message, Sprite image, KeyColor color)
    {
        toolTipManager.ShowToolTip(message, image, color);
    }

}

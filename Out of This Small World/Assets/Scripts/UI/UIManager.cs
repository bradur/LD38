// Date   : 22.04.2017 18:21
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text txtComponent;
    [SerializeField]
    private Color colorVariable;
    [SerializeField]
    private Image imgComponent;

    [SerializeField]
    private PlayerInventoryManager playerInventoryManager;

    public void InventoryGain(GenericObjectStruct genericObjectStruct)
    {
        playerInventoryManager.Gain(genericObjectStruct);
    }

}

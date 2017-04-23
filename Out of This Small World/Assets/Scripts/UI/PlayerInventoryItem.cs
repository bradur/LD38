// Date   : 22.04.2017 17:10
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInventoryItem : MonoBehaviour
{

    [SerializeField]
    private Text txtComponent;
    [SerializeField]
    private Color colorVariable;
    [SerializeField]
    private Image imgComponent;

    private GenericObjectStruct genericObjectStruct;
    public GenericObjectStruct GenericObjectStruct { get { return genericObjectStruct; } }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Init(GenericObjectStruct newObject)
    {
        genericObjectStruct = newObject;
        txtComponent.text = genericObjectStruct.name;
        imgComponent.color = genericObjectStruct.keyColor;
        imgComponent.sprite = genericObjectStruct.objectSprite;
    }
}

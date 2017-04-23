// Date   : 22.04.2017 16:53
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections;
using TiledSharp;

[RequireComponent(typeof(SpriteRenderer))]
public class GenericWorldObject : MonoBehaviour
{

    [SerializeField]
    private GenericObjectStruct genericObjectStruct;

    public GenericObjectStruct GenericObjectStruct { get { return genericObjectStruct; } }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (genericObjectStruct.pickable)
            {
                Pickup();
            }
            else
            {
                if (genericObjectStruct.objectType == ObjectType.Switch)
                {
                    ToggleSwitch();
                }
            }
        }
    }

    private bool toBeDestroyed = false;
    private SpriteRenderer sr = null;
    private BoxCollider bc = null;
    private bool firstUse = true;

    public void Init(PropertyDict dict)
    {
        if (dict.ContainsKey("SwitchId"))
        {
            genericObjectStruct.switchId = Tools.IntParseFast(dict["SwitchId"]);
        }
        if (dict.ContainsKey("Color"))
        {
            genericObjectStruct.keyColorType = (KeyColor)Tools.IntParseFast(dict["Color"]);
            genericObjectStruct.keyColor = GameManager.main.GetKeyColor(genericObjectStruct.keyColorType);
        }
        if (dict.ContainsKey("Reusable"))
        {
            genericObjectStruct.reusable = Tools.IntParseFast(dict["Reusable"]) == 1;
        }
    }

    public void Spawn(double x, double y, int width, int height)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = genericObjectStruct.objectSprite;
        sr.color = genericObjectStruct.keyColor;
        sr.sortingOrder = 5;
        transform.position = new Vector3((float)(x / 64), height - (float)(y / 64), transform.position.z);
        gameObject.name += x + ", " + y;
        if (genericObjectStruct.objectType == ObjectType.Spawn)
        {
            GameManager.main.SpawnPlayer(transform.position);
        }
    }

    public void ToggleSwitchWall(int switchId)
    {
        if (genericObjectStruct.objectType == ObjectType.SwitchWall && switchId == genericObjectStruct.switchId)
        {
            if (firstUse || genericObjectStruct.reusable)
            {
                if (sr == null)
                {
                    sr = GetComponent<SpriteRenderer>();
                }
                if (bc == null)
                {
                    bc = GetComponent<BoxCollider>();
                }
                bc.enabled = !bc.enabled;
                sr.enabled = !sr.enabled;
                firstUse = false;
            } else
            {

            }
        }
    }

    private void ToggleSwitch()
    {
        if (genericObjectStruct.switchId != -1)
        {
            GameManager.main.ToggleSwitch(genericObjectStruct.switchId);
        }
    }

    private void OnDestroy()
    {
        if (toBeDestroyed)
        {
            GameManager.main.InventoryGain(genericObjectStruct);
        }
    }

    private void Pickup()
    {
        toBeDestroyed = true;
        Destroy(gameObject);
    }
}

[System.Serializable]
public class GenericObjectStruct : System.Object
{
    public ObjectType objectType;
    public KeyColor keyColorType;
    public Color keyColor;
    public Sprite objectSprite;
    public string name;
    public int switchId = -1;
    public bool pickable = true;
    public bool reusable = false;
}

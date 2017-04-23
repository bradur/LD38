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
                else if (genericObjectStruct.objectType == ObjectType.Wormhole)
                {
                    GameManager.main.LoadNextLevel();
                }
            }
        }
    }

    private bool toBeDestroyed = false;
    private SpriteRenderer sr = null;
    private BoxCollider bc = null;
    private bool firstUse = true;
    private Color switchedOffColor;

    [SerializeField]
    private SpriteRenderer highLight;

    public void Init(PropertyDict dict)
    {
        if (dict.ContainsKey("SwitchId"))
        {
            genericObjectStruct.switchId = Tools.IntParseFast(dict["SwitchId"]);
        }
        if (dict.ContainsKey("SwitchedOn"))
        {
            genericObjectStruct.switchedOn = Tools.IntParseFast(dict["SwitchedOn"]) == 1;
        }
        if (dict.ContainsKey("Color"))
        {
            genericObjectStruct.keyColorType = (KeyColor)Tools.IntParseFast(dict["Color"]);
            Color color = GameManager.main.GetKeyColor(genericObjectStruct.keyColorType);
            if (genericObjectStruct.objectType == ObjectType.SwitchWall)
            {
                float h = 0;
                float s = 0;
                float v = 0;
                Color.RGBToHSV(color, out h, out s, out v);
                v = 0.5f;
                s = 0.5f;
                switchedOffColor = Color.HSVToRGB(h, s, v);
            }
            genericObjectStruct.keyColor = color;

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
        highLight.sprite = genericObjectStruct.objectSprite;
        sr.sortingOrder = 4;
        transform.position = new Vector3((float)(x / 64), height - (float)(y / 64), transform.position.z);
        gameObject.name += x + ", " + y;
        if (genericObjectStruct.objectType == ObjectType.Spawn)
        {
            GameManager.main.SpawnPlayer(transform.position);
        }
        if (genericObjectStruct.objectType == ObjectType.SwitchWall)
        {
            if (genericObjectStruct.switchedOn)
            {
                if (sr == null)
                {
                    sr = GetComponent<SpriteRenderer>();
                }
                if (bc == null)
                {
                    bc = GetComponent<BoxCollider>();
                }
                bc.enabled = true;
                //sr.enabled = true;
                sr.sprite = genericObjectStruct.switchSprite;
            }
            else
            {
                sr.color = switchedOffColor;
            }
        }
    }

    public void HighLight()
    {
        highLight.enabled = true;
    }

    public void LowLight()
    {
        highLight.enabled = false;
    }

    public void OpenDoor()
    {
        if (GameManager.main.InventoryUseKey(genericObjectStruct.keyColorType))
        {
            toBeDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void ChopDownTree()
    {
        toBeDestroyed = true;
        Destroy(gameObject);
    }

    public void ToggleSwitchWall(int switchId)
    {
        if (genericObjectStruct.objectType == ObjectType.SwitchWall && switchId == genericObjectStruct.switchId)
        {

            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
            }
            if (bc == null)
            {
                bc = GetComponent<BoxCollider>();
            }
            if (!bc.enabled)
            {
                sr.color = genericObjectStruct.keyColor;
                sr.sprite = genericObjectStruct.switchSprite;
            }
            else
            {
                sr.color = switchedOffColor;
                sr.sprite = genericObjectStruct.objectSprite;
            }
            bc.enabled = !bc.enabled;
            //sr.enabled = !sr.enabled;
            firstUse = false;
        }
    }

    private void ToggleSwitch()
    {
        if (genericObjectStruct.switchId != -1)
        {
            if (firstUse || genericObjectStruct.reusable)
            {
                GameManager.main.ToggleSwitch(genericObjectStruct.switchId);
                if (sr == null)
                {
                    sr = GetComponent<SpriteRenderer>();

                }
                string switchColor = genericObjectStruct.keyColorType != KeyColor.None ? genericObjectStruct.keyColorType + " " : "";
                if (sr.sprite == genericObjectStruct.objectSprite)
                {
                    GameManager.main.ShowToolTip(
                        "You walked on a " + switchColor + "switch!",
                        genericObjectStruct.switchSprite,
                        genericObjectStruct.keyColorType
                    );
                    sr.sprite = genericObjectStruct.switchSprite;
                }
                else
                {
                    GameManager.main.ShowToolTip(
                        "The switch reset!",
                        genericObjectStruct.objectSprite,
                        genericObjectStruct.keyColorType
                    );
                    sr.sprite = genericObjectStruct.objectSprite;
                }
                firstUse = false;
            }
            else
            {
                GameManager.main.ShowToolTip(
                    "The switch doesn't seem to work anymore...",
                    genericObjectStruct.switchSprite,
                    genericObjectStruct.keyColorType
                );
            }
        }
    }

    private void OnDestroy()
    {
        if (toBeDestroyed)
        {
            if (genericObjectStruct.objectType == ObjectType.Door)
            {
                GameManager.main.UpdateItems();
            }
            else if (genericObjectStruct.objectType == ObjectType.Tree)
            {
                GameManager.main.UpdateItems();
            }
            else
            {
                GameManager.main.InventoryGain(genericObjectStruct);
            }

        }
    }

    private void Pickup()
    {
        GenericObjectStruct itemStruct = null;
        if (genericObjectStruct.objectType == ObjectType.Key)
        {
            itemStruct = GameManager.main.InventoryGetKey(genericObjectStruct.keyColorType);
        }
        else
        {
            PlayerInventoryItem item = GameManager.main.InventoryGetItem(genericObjectStruct.objectType);
            if (item != null)
            {
                itemStruct = item.GenericObjectStruct;
            }
        }

        string article = "a ";
        if (genericObjectStruct.objectType == ObjectType.Axe)
        {
            article = "an ";
        }
        else if (genericObjectStruct.objectType == ObjectType.Flippers)
        {
            article = "";
        }

        if (itemStruct != null && itemStruct.keyColorType == genericObjectStruct.keyColorType)
        {

            if (genericObjectStruct.objectType == ObjectType.Flippers)
            {
                article = "another pair of ";
            }
            else
            {
                article = "another ";
            }
        }
        string itemString = genericObjectStruct.objectType.ToString().ToLower();
        GameManager.main.ShowToolTip(
            "You found " + article + (genericObjectStruct.keyColorType != KeyColor.None ? genericObjectStruct.keyColorType + " " : "") + itemString + "!",
            genericObjectStruct.objectSprite,
            genericObjectStruct.keyColorType
        );
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
    public Sprite switchSprite;
    public string name;
    public int switchId = -1;
    public bool switchedOn = false;
    public bool pickable = true;
    public bool reusable = false;
}

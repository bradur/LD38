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

    [SerializeField]
    private Sprite switchSprite;

    private Sprite originalSprite;

    [SerializeField]
    private SpriteRenderer highLight;

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
        highLight.sprite = genericObjectStruct.objectSprite;
        sr.sortingOrder = 4;
        transform.position = new Vector3((float)(x / 64), height - (float)(y / 64), transform.position.z);
        gameObject.name += x + ", " + y;
        if (genericObjectStruct.objectType == ObjectType.Spawn)
        {
            GameManager.main.SpawnPlayer(transform.position);
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
        Destroy(gameObject);
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
                if (!bc.enabled)
                {
                    GameManager.main.ShowToolTip(
                        "You walked on something and a wall sprung out from the ground!",
                        genericObjectStruct.objectSprite,
                        genericObjectStruct.keyColorType
                    );
                }
                bc.enabled = !bc.enabled;
                sr.enabled = !sr.enabled;
                firstUse = false;
            }
            else
            {

            }
        }
    }

    private void ToggleSwitch()
    {
        if (genericObjectStruct.switchId != -1)
        {
            if(firstUse || genericObjectStruct.reusable)
            {
                GameManager.main.ToggleSwitch(genericObjectStruct.switchId);
                if (sr == null)
                {
                    sr = GetComponent<SpriteRenderer>();
                    originalSprite = sr.sprite;
                }
                sr.sprite = sr.sprite != switchSprite ? switchSprite : originalSprite;
                firstUse = false;
            } else
            {
                GameManager.main.ShowToolTip(
                    "The switch doesn't seem to work anymore...",
                    switchSprite,
                    KeyColor.None
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
    public string name;
    public int switchId = -1;
    public bool pickable = true;
    public bool reusable = false;
}

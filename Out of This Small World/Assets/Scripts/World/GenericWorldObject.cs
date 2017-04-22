// Date   : 22.04.2017 16:53
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SpriteRenderer))]
public class GenericWorldObject : MonoBehaviour
{

    [SerializeField]
    private GenericObjectStruct genericObjectStruct;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Pickup();
        }
    }

    private bool weDestroying = false;

    public void Spawn(double x, double y, int width, int height)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = genericObjectStruct.objectSprite;
        sr.color = genericObjectStruct.keyColor;
        sr.sortingOrder = 5;
        transform.position = new Vector3((float)(x / 64), height - (float)(y / 64), transform.position.z);
        Logger.Log("x[" + x + "] y[" + y + "]");
        gameObject.name += x + ", " + y;
        if (genericObjectStruct.objectType == ObjectType.Spawn)
        {
            GameManager.main.SpawnPlayer(transform.position);
        }
    }

    private void OnDestroy()
    {
        if (weDestroying)
        {
            GameManager.main.InventoryGain(genericObjectStruct);
        }
    }

    private void Pickup()
    {
        weDestroying = true;
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
}

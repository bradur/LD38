// Date   : 22.04.2017 07:40
// Project: Out of This Small World
// Author : bradur

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    private Vector3 direction;
    private Rigidbody rigidBody;

    [Range(20f, 300f)]
    [SerializeField]
    private float forwardSpeed = 1f;

    [Range(20f, 300f)]
    [SerializeField]
    private float backwardSpeed = 1f;

    [Range(20f, 300f)]
    [SerializeField]
    private float rotationSpeed = 100f;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    GenericWorldObject doorToOpen = null;

    private void FixedUpdate()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        if (horizontalAxis != 0)
        {
            float horizontalRotation = horizontalAxis * Time.deltaTime * rotationSpeed;
            //transform.Rotate(0f, 0f, -horizontalRotation);
            transform.eulerAngles -= new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, horizontalRotation);
        }

        float verticalAxis = Input.GetAxis("Vertical");

        if (verticalAxis > 0)
        {
            rigidBody.AddForce(transform.right * forwardSpeed, ForceMode.Force);
        }
        else if (verticalAxis < 0)
        {
            rigidBody.AddForce(-transform.right * forwardSpeed, ForceMode.Force);
        }
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyManager.main.GetKey(Action.UseKeyOnDoor)) && doorToOpen != null)
        {
            if (Vector3.Distance(transform.position, doorToOpen.transform.position) < 1f)
            {
                doorToOpen.OpenDoor();
                GameManager.main.ShowToolTip(
                    "You unlocked the " + doorToOpen.GenericObjectStruct.keyColorType + " door!",
                    doorToOpen.GenericObjectStruct.objectSprite,
                    doorToOpen.GenericObjectStruct.keyColorType
                );
            } else
            {
                GameManager.main.ShowToolTip(
                    "Too far away to unlock the " + doorToOpen.GenericObjectStruct.keyColorType + " door!",
                    doorToOpen.GenericObjectStruct.objectSprite,
                    doorToOpen.GenericObjectStruct.keyColorType
                );
                doorToOpen = null;
            }
        }
        if (Input.GetKeyUp(KeyManager.main.GetKey(Action.SwingAxe)))
        {
            PlayerInventoryItem item = GameManager.main.InventoryGetItem(ObjectType.Axe);
            if(item != null)
            {
                GameManager.main.InventoryUseItem(item);
                SwingAxe();
                GameManager.main.KillToolTip();
            }
        }
    }

    private void SwingAxe()
    {
        Logger.Log("Swing!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        GenericWorldObject worldObject = collision.gameObject.GetComponent<GenericWorldObject>();
        if (collision.gameObject.tag == "Door")
        {
            GenericObjectStruct keyStruct = GameManager.main.InventoryGetKey(worldObject.GenericObjectStruct.keyColorType);
            if (keyStruct != null)
            {
                GameManager.main.ShowToolTip(
                    "Press " + KeyManager.main.GetKeyString(Action.UseKeyOnDoor) + " to open the door with your " + keyStruct.keyColorType + " key.",
                    keyStruct.objectSprite,
                    keyStruct.keyColorType
                );
                doorToOpen = worldObject;
            }
            else
            {
                GenericWorldObject keyObject = GameManager.main.GetWorldObjectPrefab(ObjectType.Key);
                GameManager.main.ShowToolTip(
                    "Find a " + worldObject.GenericObjectStruct.keyColorType + " key.",
                    keyObject.GenericObjectStruct.objectSprite,
                    worldObject.GenericObjectStruct.keyColorType
                );
            }
        }
        else if (collision.gameObject.tag == "Water")
        {
            GenericWorldObject flippersObject = GameManager.main.GetWorldObjectPrefab(ObjectType.Flippers);
            GameManager.main.ShowToolTip(
                "Find a pair of flippers.",
                flippersObject.GenericObjectStruct.objectSprite,
                KeyColor.None
            );
        }
        else if (collision.gameObject.tag == "Tree")
        {
            PlayerInventoryItem item = GameManager.main.InventoryGetItem(ObjectType.Axe);
            if (item != null)
            {
                GameManager.main.ShowToolTip(
                    "Press " + KeyManager.main.GetKeyString(Action.SwingAxe) + " to swing your axe.",
                    item.GenericObjectStruct.objectSprite,
                    KeyColor.None
                );
            }
            else
            {
                GenericWorldObject axeObject = GameManager.main.GetWorldObjectPrefab(ObjectType.Axe);
                GameManager.main.ShowToolTip(
                    "Find an axe. Or find some other way...",
                    axeObject.GenericObjectStruct.objectSprite,
                    worldObject.GenericObjectStruct.keyColorType
                );
            }
        }
    }

}

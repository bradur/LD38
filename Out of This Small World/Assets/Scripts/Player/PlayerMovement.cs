// Date   : 22.04.2017 07:40
// Project: Out of This Small World
// Author : bradur

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    private Vector3 direction;
    private Rigidbody rigidBody;

    [Range(20f, 500f)]
    [SerializeField]
    private float forwardSpeed = 350f;

    [Range(20f, 800f)]
    [SerializeField]
    private float sprintSpeed = 600f;

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

    GenericWorldObject targetTree = null;
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
            float speed = forwardSpeed;
            if (Input.GetKey(KeyManager.main.GetKey(Action.Sprint)))
            {
                speed = sprintSpeed;
                Logger.Log(speed + "");
            }
            rigidBody.AddForce(transform.right * speed, ForceMode.Force);
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
            if (Vector2.Distance(transform.position, doorToOpen.transform.position) < 1f)
            {
                doorToOpen.OpenDoor();
                GameManager.main.ShowToolTip(
                    "You unlocked the " + doorToOpen.GenericObjectStruct.keyColorType + " door!",
                    doorToOpen.GenericObjectStruct.objectSprite,
                    doorToOpen.GenericObjectStruct.keyColorType
                );
            }
            else
            {
                GameManager.main.ShowToolTip(
                    "Too far away to unlock the " + doorToOpen.GenericObjectStruct.keyColorType + " door!",
                    doorToOpen.GenericObjectStruct.objectSprite,
                    doorToOpen.GenericObjectStruct.keyColorType
                );
                doorToOpen.LowLight();
                doorToOpen = null;
            }
        }
        if (Input.GetKeyUp(KeyManager.main.GetKey(Action.SwingAxe)) && targetTree != null)
        {
            PlayerInventoryItem item = GameManager.main.InventoryGetItem(ObjectType.Axe);
            if (item != null)
            {
                if (Vector2.Distance(transform.position, targetTree.transform.position) < 1f)
                {
                    GameManager.main.InventoryUseItem(item);
                    GameManager.main.ShowToolTip(
                        "You chopped down the tree. The axe was so poorly made it broke!",
                        targetTree.GenericObjectStruct.objectSprite,
                        targetTree.GenericObjectStruct.keyColorType
                    );
                    SwingAxe();
                }
                else
                {
                    GameManager.main.ShowToolTip(
                        "Too far away to chop the tree!",
                        targetTree.GenericObjectStruct.objectSprite,
                        targetTree.GenericObjectStruct.keyColorType
                    );
                    targetTree.LowLight();
                    targetTree = null;
                }

            }
        }
        if (targetTree)
        {
            if (Vector2.Distance(transform.position, targetTree.transform.position) > 1f)
            {
                targetTree.LowLight();
                targetTree = null;
            }
        }
        if (doorToOpen)
        {
            if (Vector2.Distance(transform.position, doorToOpen.transform.position) > 1f)
            {
                doorToOpen.LowLight();
                doorToOpen = null;
            }
        }
    }

    private void SwingAxe()
    {
        targetTree.ChopDownTree();
        targetTree = null;
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
                doorToOpen.HighLight();
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
                "Find a pair of flippers if you want to swim.",
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
                    "Press " + KeyManager.main.GetKeyString(Action.SwingAxe) + " to chop down the tree with your axe.",
                    item.GenericObjectStruct.objectSprite,
                    KeyColor.None
                );
                if (targetTree != null)
                {
                    targetTree.LowLight();
                }
                targetTree = worldObject;
                targetTree.HighLight();
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
        else if (collision.gameObject.tag == "SwitchWall")
        {
            GenericWorldObject switchObject = GameManager.main.GetWorldObjectPrefab(ObjectType.Switch);
            string switchColor = worldObject.GenericObjectStruct.keyColorType != KeyColor.None ? worldObject.GenericObjectStruct.keyColorType  + " " : "";
            GameManager.main.ShowToolTip(
                "Walk on a " + switchColor + "switch to get through here.",
                switchObject.GenericObjectStruct.objectSprite,
                worldObject.GenericObjectStruct.keyColorType
            );
        }
    }

}

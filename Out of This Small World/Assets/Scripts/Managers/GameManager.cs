// Date   : 22.04.2017 17:36
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager main;


    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private LoopLevelLoader loopLevelLoader;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("GameManager").Length < 1)
        {
            gameObject.tag = "GameManager";
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleSwitch(int switchId)
    {
        loopLevelLoader.ToggleSwitch(switchId);
    }

    public void InventoryGain(GenericObjectStruct genericObjectStruct)
    {
        uiManager.InventoryGain(genericObjectStruct);
        if (genericObjectStruct.objectType == ObjectType.Flippers)
        {
            loopLevelLoader.FlippersFound();
        }
        loopLevelLoader.UpdateItems();
    }

    public void SpawnPlayer(Vector3 spawnPosition)
    {
        playerTransform.position = spawnPosition;
    }

    public void PlayerIsSwimming()
    {
        Logger.Log("Swimming!");
    }

    public void PlayerIsLeavingAWaterTile()
    {
        Logger.Log("Not swimming?");
    }
}

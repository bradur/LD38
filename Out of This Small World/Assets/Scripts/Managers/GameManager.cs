// Date   : 22.04.2017 17:36
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager main;

    private UIManager uiManager;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private LoopLevelLoader loopLevelLoader;

    [SerializeField]
    private LevelLoader levelLoader;

    [SerializeField]
    private List<Color> keyColors = new List<Color>();

    [SerializeField]
    private Sprite exitSprite;

    private bool waitingForExitKey = false;

    [SerializeField]
    private AudioSource themeSong;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("GameManager").Length < 1)
        {
            gameObject.tag = "GameManager";
            uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    public void LoadNextLevel()
    {
        uiManager.ClearInventory();
        uiManager.KillToolTip();
        levelLoader.LoadNextLevel();
    }

    public GenericWorldObject GetWorldObjectPrefab(ObjectType objectType)
    {
        return levelLoader.GetWorldObjectPrefab(objectType);
    }

    public void ShowToolTip(string message, Sprite sprite, KeyColor color)
    {
        waitingForExitKey = false;
        uiManager.ShowToolTip(message, sprite, color);
    }

    public GenericObjectStruct InventoryGetKey(KeyColor color)
    {
        return uiManager.InventoryGetKey(color);
    }

    public PlayerInventoryItem InventoryGetItem(ObjectType objectType)
    {
        return uiManager.InventoryGetItem(objectType);
    }

    public void InventoryUseItem(PlayerInventoryItem item)
    {
        uiManager.InventoryUseItem(item);
    }

    public bool InventoryUseKey(KeyColor color)
    {
        return uiManager.InventoryUseKey(color);
    }

    public void KillToolTip()
    {
        uiManager.KillToolTip();
    }

    public Color GetKeyColor(KeyColor color)
    {
        return keyColors[(int)color];
    }

    public void ToggleSwitch(int switchId)
    {
        loopLevelLoader.ToggleSwitch(switchId);
    }

    public void UpdateItems()
    {
        loopLevelLoader.UpdateItems();
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
    }

    public void PlayerIsLeavingAWaterTile()
    {
    }

    public void StartWaitingForExitKey()
    {
        waitingForExitKey = true;
    }

    public void StopWaitingForExitKey()
    {
        waitingForExitKey = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyManager.main.GetKey(Action.OpenExitMenu)))
        {
            ShowToolTip(
                "Press " + KeyManager.main.GetKey(Action.Exit) + " to quit or " + KeyManager.main.GetKey(Action.Restart) + " to restart level.",
                exitSprite,
                KeyColor.None
            );
            waitingForExitKey = true;
        }
        if (waitingForExitKey)
        {
            if (Input.GetKeyUp(KeyManager.main.GetKey(Action.Exit)))
            {
                Application.Quit();
            }
            if (Input.GetKeyUp(KeyManager.main.GetKey(Action.Restart)))
            {
                uiManager.ClearInventory();
                uiManager.KillToolTip();
                levelLoader.RestartLevel();
            }
        }

        if (Input.GetKeyUp(KeyManager.main.GetKey(Action.ToggleAudio)))
        {
            ShowToolTip(
                "All audio is now: " + (themeSong.isPlaying ? "OFF" : "ON"),
                exitSprite,
                KeyColor.None
            );
            if (themeSong.isPlaying)
            {
                themeSong.Pause();
            } else
            {
                themeSong.Play();
            }
            SoundManager.main.Toggle();
        }
    }
}

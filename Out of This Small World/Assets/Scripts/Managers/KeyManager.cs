// Date   : 23.04.2017 11:09
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections.Generic;

public enum Action
{
    None,
    SwingAxe,
    UseKeyOnDoor,
    Sprint,
    OpenExitMenu,
    Exit,
    Restart
}

public class KeyManager : MonoBehaviour {


    public static KeyManager main;

    private void Awake()
    {
        main = this;
    }

    [SerializeField]
    private List<GameKey> gameKeys = new List<GameKey>();

    public KeyCode GetKey(Action action)
    {
        foreach (GameKey gameKey in gameKeys)
        {
            if (gameKey.action == action)
            {
                return gameKey.key;
            }
        }
        return KeyCode.None;
    }

    public string GetKeyString(Action action)
    {
        foreach (GameKey gameKey in gameKeys)
        {
            if (gameKey.action == action)
            {
                string keyString = gameKey.key.ToString();
                if (gameKey.key == KeyCode.Return)
                {
                    keyString = "Enter";
                } else if (gameKey.key == KeyCode.RightControl)
                {
                    keyString = "Right Ctrl";
                }
                return keyString;
            }
        }
        return "";
    }
}
[System.Serializable]
public class GameKey : System.Object
{
    public KeyCode key;
    public Action action;
}
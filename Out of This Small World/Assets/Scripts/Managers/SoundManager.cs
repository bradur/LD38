// Date   : 24.04.2017 01:20
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections.Generic;


public enum SoundType
{
    None,
    ItemPickup,
    ChopWood,
    OpenDoor,
    WalkWormhole,
    Switch
}

public class SoundManager : MonoBehaviour {

    public static SoundManager main;

    [SerializeField]
    private List<GameSound> sounds = new List<GameSound>();

    private bool isOn = true;

    void Awake()
    {
        main = this;
    }

    public void PlaySound(SoundType soundType)
    {
        if (isOn)
        {
            foreach (GameSound gameSound in sounds)
            {
                if (gameSound.soundType == soundType)
                {
                    gameSound.sound.Play();
                }
            }
        }
    }

    public bool Toggle()
    {
        isOn = !isOn;
        return isOn;
    }
}

[System.Serializable]
public class GameSound : System.Object
{

    public SoundType soundType;
    public AudioSource sound;

}
// Date   : 22.04.2017 09:44
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections;

public class LoopLevelLoader : MonoBehaviour {

    private World currentWorld;

    private World[][] maps = new World[3][] {
        new World[] {null, null, null},
        new World[] {null, null, null},
        new World[] {null, null, null}
    };

    private int mapWidth;
    private int mapHeight;

    public void Init (World initialWorld, int width, int height)
    {
        currentWorld = initialWorld;
        currentWorld.IsCurrentWorld = true;
        maps[1][1] = currentWorld;
        mapWidth = width;
        mapHeight = height;
        LoadNewMaps();
    }

    private void LoadNewMaps()
    {
        for (int i = 0; i < maps.Length; i += 1)
        {
            for (int j = 0; j < maps[i].Length; j += 1)
            {
                LoadMapIfNotLoaded(j, i);
            }
        }
    }

    public void FlippersFound() {
        foreach (GameObject waterObject in GameObject.FindGameObjectsWithTag("Water"))
        {
            waterObject.GetComponent<Water>().GainFlippers();
        }
    }

    public void ToggleSwitch(int switchId)
    {
        GameObject items = currentWorld.GetItemContainer();
        foreach(Transform child in items.transform)
        {
            GenericWorldObject worldObject = child.GetComponent<GenericWorldObject>();
            worldObject.ToggleSwitchWall(switchId);
        }
        UpdateItems();
    }

    public void UpdateItems()
    {
        GameObject items = currentWorld.GetItemContainer();
        for (int i = 0; i < maps.Length; i += 1)
        {
            for (int j = 0; j < maps[i].Length; j += 1)
            {
                if (maps[i][j] != null && !maps[i][j].IsCurrentWorld)
                {
                    maps[i][j].SetItems(items);
                }
            }
        }
    }

    public void PlayerHitWorld(World world) {
        if (!world.IsCurrentWorld)
        {
            currentWorld.IsCurrentWorld = false;
            currentWorld = world;
            ShiftMaps(currentWorld.X - 1, currentWorld.Y - 1);
            currentWorld.IsCurrentWorld = true;
            LoadNewMaps();
        }
    }

    private World CloneCurrentMap(int x, int y)
    {
        World newWorld = Instantiate(currentWorld);
        newWorld.X = x;
        newWorld.Y = y;
        newWorld.transform.parent = currentWorld.transform.parent;
        newWorld.transform.position = new Vector3(
            currentWorld.transform.position.x + (x - 1) * mapWidth,
            currentWorld.transform.position.y + -(y - 1) * mapHeight,
            currentWorld.transform.position.z
        );
        return newWorld;
    }

    public void LoadMapIfNotLoaded (int x, int y)
    {
        if (maps.Length >= y)
        {
            if (maps[y].Length >= x)
            {
                if (maps[y][x] == null)
                {
                    maps[y][x] = CloneCurrentMap(x, y);
                }
            }
        }
    }

    private void ShiftMaps(int x, int y)
    {
        World[][] newMaps = new World[3][] {
            new World[] {null, null, null},
            new World[] {null, null, null},
            new World[] {null, null, null}
        };
        for (int indexY = 0; maps.Length > indexY; indexY += 1) {
            int newY = indexY - y;
            for (int indexX = 0; maps[indexY].Length > indexX; indexX += 1)
            {
                int newX = indexX - x;
                if (newY >= 0 && newY < newMaps.Length && newX >= 0 && newX < newMaps[newY].Length)
                {
                    newMaps[newY][newX] = maps[indexY][indexX];
                    if(newMaps[newY][newX] != null) {
                        newMaps[newY][newX].X = newX;
                        newMaps[newY][newX].Y = newY;
                    }
                } else
                {
                    if(maps[indexY][indexX] != null && !maps[indexY][indexX].IsCurrentWorld)
                    {
                        Destroy(maps[indexY][indexX].gameObject);
                    }
                }
            }
        }
        maps = newMaps;
    }

}

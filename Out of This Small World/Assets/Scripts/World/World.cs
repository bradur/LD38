// Date   : 22.04.2017 09:54
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections;
public class World : MonoBehaviour
{

    private int x;
    public int X { get { return x; } set { x = value; } }
    private int y;
    public int Y { get { return y; } set { y = value; } }

    [HideInInspector]
    public LoopLevelLoader loopLevelLoader;

    private bool isCurrentMap = false;
    public bool IsCurrentWorld { get { return isCurrentMap; } set { isCurrentMap = value; } }

    public void Init(int xPos, int yPos, LoopLevelLoader loader)
    {
        x = xPos;
        y = yPos;
        loopLevelLoader = loader;
    }

    public void PlayerHitWorld()
    {
        loopLevelLoader.PlayerHitWorld(this);
    }

    public void PlayerHitBorder(Direction direction)
    {
        int nextX = 0;
        int nextY = 0;
        if (direction == Direction.NorthWest)
        {
            nextX = 0;
            nextY = 0;
        }
        else if (direction == Direction.North)
        {
            nextX = 1;
            nextY = 0;
        }
        else if (direction == Direction.NorthEast)
        {
            nextX = 2;
            nextY = 0;
        }
        else if (direction == Direction.East)
        {
            nextX = 2;
            nextY = 1;
        }
        else if (direction == Direction.SouthEast)
        {
            nextX = 2;
            nextY = 2;
        }
        else if (direction == Direction.South)
        {
            nextX = 1;
            nextY = 2;
        }
        else if (direction == Direction.SouthWest)
        {
            nextX = 0;
            nextY = 2;
        }
        else if (direction == Direction.West)
        {
            nextX = 0;
            nextY = 1;
        }
        loopLevelLoader.LoadMapIfNotLoaded(nextX, nextY);
    }

}

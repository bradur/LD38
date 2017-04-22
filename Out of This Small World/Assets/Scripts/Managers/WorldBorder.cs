// Date   : 22.04.2017 11:29
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class WorldBorder : MonoBehaviour
{

    [HideInInspector]
    public World parentWorld;
    [HideInInspector]
    public Direction direction;

    public void Init(World world, Direction dir)
    {
        parentWorld = world;
        direction = dir;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && parentWorld.IsCurrentWorld)
        {
            parentWorld.PlayerHitBorder(direction);
        }
    }
}

// Date   : 22.04.2017 13:02
// Project: Out of This Small World
// Author : bradur

using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class WorldCollider : MonoBehaviour
{

    [HideInInspector]
    public World parentWorld;

    public void Init(World world)
    {
        parentWorld = world;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Logger.Log("playerhitWorld!");
            parentWorld.PlayerHitWorld();
        }
    }
}

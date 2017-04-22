// Date   : 22.04.2017 23:10
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{

    [SerializeField]
    BoxCollider boxCollider;

    public void GainFlippers()
    {
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameManager.main.PlayerIsSwimming();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            GameManager.main.PlayerIsLeavingAWaterTile();
        }
    }
}

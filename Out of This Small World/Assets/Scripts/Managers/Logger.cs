// Date   : 22.04.2017 09:47
// Project: Out of This Small World
// Author : bradur

using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField]
    private static bool allowLogging = true;

    public static void Log (string message)
    {
        if (allowLogging)
        {
            Debug.Log(message);
        }
    }
}

// Date   : 23.04.2017 09:37
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolTipManager : MonoBehaviour
{

    private UIToolTip currentToolTip = null;

    [SerializeField]
    private UIToolTip toolTipPrefab;

    public void ShowToolTip(string message, Sprite sprite, KeyColor color)
    {
        if (currentToolTip == null)
        {
            currentToolTip = Instantiate(toolTipPrefab);
            currentToolTip.transform.SetParent(transform, false);
            currentToolTip.Init(message, sprite, color);
        }
        else
        {
            if (currentToolTip.Message != message)
            {
                if (currentToolTip != null)
                {
                    currentToolTip.Kill();
                }
                currentToolTip = Instantiate(toolTipPrefab);
                currentToolTip.transform.SetParent(transform, false);
                currentToolTip.Init(message, sprite, color);
            }
        }
    }

    public void KillToolTip()
    {
        currentToolTip.Kill();
    }
}

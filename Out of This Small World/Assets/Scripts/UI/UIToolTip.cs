// Date   : 23.04.2017 09:21
// Project: Out of This Small World
// Author : bradur

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIToolTip : MonoBehaviour {

    [SerializeField]
    private Text txtComponent;
    [SerializeField]
    private Image imgComponent;

    private Animator animator;

    private string msg;
    public string Message { get { return msg; } }

    public void Init(string message, Sprite sprite, KeyColor color)
    {
        animator = GetComponent<Animator>();
        msg = message;
        txtComponent.text = message;
        imgComponent.sprite = sprite;
        if (color != KeyColor.None) { 
            imgComponent.color = GameManager.main.GetKeyColor(color);
        }
        animator.SetTrigger("Start");
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

}

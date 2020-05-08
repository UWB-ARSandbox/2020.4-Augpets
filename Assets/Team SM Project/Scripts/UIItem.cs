using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code from https://medium.com/@yonem9/create-an-unity-inventory-part-4-display-items-in-ui-6cdac8f734b7

public class UIItem : MonoBehaviour
{
    public Item item;
    private Image spriteImage;
    private Image backgroundImage;

    public void Awake()
    {
        backgroundImage = gameObject.transform.parent.GetComponent<Image>();
        spriteImage = GetComponent<Image>();
    }

    public void UpdateItem(Item item)
    {
        this.item = item;
        if(this.item != null)
        {
            if(this.item.placed)
            {
                backgroundImage.color = Color.gray;
                spriteImage.color = Color.black;
                spriteImage.sprite = this.item.icon;
            }
            else
            {
                backgroundImage.color = Color.white;
                spriteImage.color = Color.white;
                spriteImage.sprite = this.item.icon;
            }
        }
        else
        {
            spriteImage.color = Color.clear;
        }
    }
}

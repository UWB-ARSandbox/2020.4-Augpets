using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum message
{
    AnimationStarted,
    AnimationEnded
}

public class InventoryDisplay : MonoBehaviour
{
    public GameObject Collection;

    public void OpenBackpack()
    {
        if(Collection != null)
        {
            Animator animator = Collection.GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen = animator.GetBool("open");

                animator.SetBool("open", !isOpen);
            }
        }
    }
}

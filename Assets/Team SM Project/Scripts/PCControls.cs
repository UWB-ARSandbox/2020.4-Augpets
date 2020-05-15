using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
public class PCControls : MonoBehaviour
{
/*    ARObjectInteraction interaction;
    Vector2 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        interaction = this.GetComponent<ARObjectInteraction>();
        mousePosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            mousePosition = Input.mousePosition;

            if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(PointerId.mousePointerId))
            {
                if(interaction.interactMode)
                {
                    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                    RaycastHit hitObject;
                    int layerMask = 1 << 9;
                    layerMask = ~layerMask;

                    // Object touched
                    if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                    {
                        if (hitObject.collider != null && hitObject.collider.gameObject.name.Contains("PlatformPlane"))
                        {
                            if (interaction.CanPlaceObject())
                            {
                                ASL.ASLHelper.InstanitateASLObject(interaction.objectToPlace, (hitObject.point), (hitObject.collider.transform.rotation));
                            }
                        }
                    }
                }
            }
        }

    }*/
}

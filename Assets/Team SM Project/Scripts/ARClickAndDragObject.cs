using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARClickAndDragObject : MonoBehaviour
{
    private Vector2 touchPosition;

    private ARRaycastManager _arRaycastManager;
    private GameObject _arSessionOrigin;

    private GameObject selectedObject;

    private bool onTouchHold = false;

    private bool createMode = false;

    public Text selectedObjectDisplay;
    public Button createButton;
    public Button moveButton;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {

    }

    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin");
        _arRaycastManager = _arSessionOrigin.GetComponent<ARRaycastManager>();

        createButton.onClick.AddListener(SetCreateMode);
        moveButton.onClick.AddListener(SetMoveMode);
    }

    private void SetCreateMode()
    {
        createMode = true;
        DeselectObject();
    }

    private void SetMoveMode()
    {
        createMode = false;
        DeselectObject();
    }

    private void SelectObject(GameObject objectToSelect)
    {
        DeselectObject();
        selectedObject = objectToSelect;
        if (selectedObject != null)
        {
            //onTouchHold = true;
            selectedObject.GetComponent<Renderer>().material.color = Color.red;
            selectedObjectDisplay.text = "Selected: " + selectedObject.transform.gameObject.name;
        }
    }

    private void DeselectObject()
    {
        selectedObject = null;
        selectedObject.GetComponent<Renderer>().material.color = Color.white;
        selectedObjectDisplay.text = "Selected: None";
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                if(createMode)
                {
                    if (_arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        var hitPose = hits[0].pose;
                        ASL.ASLHelper.InstanitateASLObject("Character", hitPose.position, hitPose.rotation);
                    }
                }

                if(!createMode)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitObject;

                    // Plane touched
                    if (_arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
                    {
                        // Object selected
                        if(selectedObject != null)
                        {
                            var hitPose = hits[0].pose;
                            selectedObject.transform.position = hitPose.position;
                        }
                        else
                        {
                            DeselectObject();
                        }
                    }
                    
                    // Object touched
                    if (Physics.Raycast(ray, out hitObject))
                    {
                        SelectObject(hitObject.transform.gameObject);
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                //onTouchHold = false;
                DeselectObject();
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

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
            selectedObject.GetComponent<Renderer>().material.color = Color.red;
            selectedObjectDisplay.text = "Selected: " + selectedObject.name;
        }
    }
    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<Renderer>().material.color = Color.white;
            selectedObjectDisplay.text = "Selected: None";
            selectedObject = null;
        }
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;
            // Touch started and not held
            if (touch.phase == TouchPhase.Began && !onTouchHold && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // Create mode
                if (createMode)
                {
                    // Instantiate new pet
                    Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                    if (hitPose != null)
                    {
                        ASL.ASLHelper.InstanitateASLObject("Character", (((Pose)hitPose).position), (((Pose)hitPose).rotation));
                    }
                }
                // Move mode
                if (!createMode)
                {
                    Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                    RaycastHit hitObject;
                    int layerMask = 1 << 9;
                    layerMask = ~layerMask;
                    // Object touched
                    if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                    {
                        if (hitObject.collider != null)
                        {
                            SelectObject(hitObject.collider.gameObject);
                        }
                    }
                }
                // Holding touch until touch phase ends
                onTouchHold = true;
            }
            // Holding touch
            else if (onTouchHold && selectedObject != null)
            {
                Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                if (hitPose != null)
                {
                    selectedObject.transform.position = ((Pose)hitPose).position;
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
                DeselectObject();
            }
        }
    }
}
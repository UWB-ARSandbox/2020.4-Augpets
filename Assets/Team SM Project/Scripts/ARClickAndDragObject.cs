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

    /*private GameObject selectedObject;

    private bool onTouchHold = false;

    private bool createMode = false;


    public Text selectedObjectDisplay;
    public Button createButton;
    public Button moveButton;*/

    private int selectedSlot = 1;
    private string objectToPlace = "";

    public Button slot1;
    public int slot1Inventory = 3;
    public Text slot1Text;
    public Button slot2;
    public int slot2Inventory = 3;
    public Text slot2Text;
    public Button slot3;
    public int slot3Inventory = 3;
    public Text slot3Text;
    public Button slot4;
    public int slot4Inventory = 3;
    public Text slot4Text;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {

    }

    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin");
        _arRaycastManager = _arSessionOrigin.GetComponent<ARRaycastManager>();

        //createButton.onClick.AddListener(SetCreateMode);
        //moveButton.onClick.AddListener(SetMoveMode);

        slot1.onClick.AddListener(SetSlot1);
        slot2.onClick.AddListener(SetSlot2);
        slot3.onClick.AddListener(SetSlot3);
        slot4.onClick.AddListener(SetSlot4);
    }

    /*private void SetCreateMode()
    {
        createMode = true;
        DeselectObject();
    }

    private void SetMoveMode()
    {
        createMode = false;
        DeselectObject();
    }*/

    private void SetSlot1()
    {
        selectedSlot = 1;
        objectToPlace = "Capsule Pet";
    }

    private void SetSlot2()
    {
        selectedSlot = 2;
        objectToPlace = "Cube Pet";
    }

    private void SetSlot3()
    {
        selectedSlot = 3;
        objectToPlace = "Cylinder Pet";
    }

    private void SetSlot4()
    {
        selectedSlot = 4;
        objectToPlace = "Sphere Pet";
    }

    private bool CanPlaceObject()
    {
        switch (selectedSlot)
        {
            case 1:
                if(slot1Inventory > 0)
                {
                    slot1Inventory--;
                    slot1Text.text = slot1Inventory.ToString();
                    return true;
                }
                break;
            case 2:
                if (slot2Inventory > 0)
                {
                    slot2Inventory--;
                    slot2Text.text = slot2Inventory.ToString();
                    return true;
                }
                break;
            case 3:
                if (slot3Inventory > 0)
                {
                    slot3Inventory--;
                    slot3Text.text = slot3Inventory.ToString();
                    return true;
                }
                break;
            case 4:
                if (slot4Inventory > 0)
                {
                    slot4Inventory--;
                    slot4Text.text = slot4Inventory.ToString();
                    return true;
                }
                break;
            default:
                return false;
        }
        return false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            // Touch started and not held
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // Instantiate new pet
                Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                if (hitPose != null)
                {
                    if(CanPlaceObject())
                    {
                        ASL.ASLHelper.InstanitateASLObject(objectToPlace, (((Pose)hitPose).position), (((Pose)hitPose).rotation));
                    }
                }
            }
        }
    }

    /*private void SelectObject(GameObject objectToSelect)
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
        if(selectedObject != null)
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
                if(createMode)
                {
                    // Instantiate new pet
                    Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                    if (hitPose != null)
                    {
                        ASL.ASLHelper.InstanitateASLObject("Character", (((Pose)hitPose).position), (((Pose)hitPose).rotation));
                    }
                }

                // Move mode
                if(!createMode)
                {
                    Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);

                    Ray ray = Camera.main.ScreenPointToRay(((Pose)hitPose).position);
                    RaycastHit hitObject;

                    int layerMask = 1 << 9;
                    layerMask = ~layerMask;

                    // Object touched
                    if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                    {
                        if(hitObject.collider != null)
                        {
                            SelectObject(hitObject.collider.gameObject);
                        }
                    }
                }

                // Holding touch until touch phase ends
                onTouchHold = true;
            }
            // Holding touch
            else if(onTouchHold && selectedObject != null)
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
    }*/
}
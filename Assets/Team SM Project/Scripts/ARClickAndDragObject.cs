using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

public class ARClickAndDragObject : MonoBehaviour
{
    private ARRaycastManager _arRaycastManager;
    private GameObject _arSessionOrigin;

    private GameObject selectedObject;

    private Vector2 touchPosition;
    private bool onTouchHold = false;

    private bool interactMode = true;

    public Button interactButton;
    public Button removeButton;

    private int selectedSlot = 1;
    private string objectToPlace = "Capsule";

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

    public Text selectedObjectDisplay;
    public Image selectedSlotDisplay;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {

    }

    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin");
        _arRaycastManager = _arSessionOrigin.GetComponent<ARRaycastManager>();

        interactButton.onClick.AddListener(SetInteractMode);
        removeButton.onClick.AddListener(SetRemoveMode);

        slot1.onClick.AddListener(SetSlot1);
        slot2.onClick.AddListener(SetSlot2);
        slot3.onClick.AddListener(SetSlot3);
        slot4.onClick.AddListener(SetSlot4);

        selectedObjectDisplay.text = objectToPlace;
    }

    private void SetInteractMode()
    {
        interactMode = true;
        DeselectObject();
    }

    private void SetRemoveMode()
    {
        interactMode = false;
        DeselectObject();
    }

    private void SetSlot1()
    {
        selectedSlot = 1;
        objectToPlace = "Capsule";
        selectedSlotDisplay.transform.position = slot1.transform.position;
        selectedObjectDisplay.text = objectToPlace;
    }

    private void SetSlot2()
    {
        selectedSlot = 2;
        objectToPlace = "Cube";
        selectedSlotDisplay.transform.position = slot2.transform.position;
        selectedObjectDisplay.text = objectToPlace;
    }

    private void SetSlot3()
    {
        selectedSlot = 3;
        objectToPlace = "Cylinder";
        selectedSlotDisplay.transform.position = slot3.transform.position;
        selectedObjectDisplay.text = objectToPlace;
    }

    private void SetSlot4()
    {
        selectedSlot = 4;
        objectToPlace = "Sphere";
        selectedSlotDisplay.transform.position = slot4.transform.position;
        selectedObjectDisplay.text = objectToPlace;
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

    /*private void GetSlot(GameObject button)
    {
        switch (button)
        {
            case button == slot1:
                SetSlot1();
                break;
            case button == slot2:
                SetSlot2();
                break;
            case button == slot3:
                SetSlot3();
                break;
            case button == slot4:
                SetSlot4();
                break;
        }
    }*/

    private void SelectObject(GameObject objectToSelect)
    {
        DeselectObject();
        selectedObject = objectToSelect;
        if (selectedObject != null)
        {
            selectedObject.GetComponent<Renderer>().material.color = Color.red;
            selectedObjectDisplay.text = selectedObject.name;
        }
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            selectedObject.GetComponent<Renderer>().material.color = Color.white;
            selectedObjectDisplay.text = objectToPlace;
            selectedObject = null;
        }
    }

    private void RemoveObject(GameObject objectToRemove)
    {
        if (objectToRemove != null)
        {
            //Destroy(objectToRemove);
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            // Touch started and not held, place
            if (touch.phase == TouchPhase.Began && !onTouchHold && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // Interact mode
                if(interactMode)
                {
                    // Instantiate new pet
                    Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                    if (hitPose != null)
                    {
                        if (CanPlaceObject())
                        {
                            ASL.ASLHelper.InstanitateASLObject(objectToPlace, (((Pose)hitPose).position), (((Pose)hitPose).rotation));
                        }
                    }
                }
                // Remove mode
                else
                {
                    // Remove pet and add back to inventory
                    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                    RaycastHit hitObject;
                    int layerMask = 1 << 9;
                    layerMask = ~layerMask;

                    // Object touched
                    if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                    {
                        if (hitObject.collider != null)
                        {
                            RemoveObject(hitObject.collider.gameObject);
                        }
                    }
                }
                onTouchHold = true;
            }
            // Touch held on plane, select object
            else if (selectedObject == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;
                int layerMask = 1 << 9;
                layerMask = ~layerMask;

                // Object touched
                if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                {
                    if (hitObject.collider != null)
                    {
                        if (!hitObject.collider.gameObject.name.Contains("ARPlane"))
                        {
                            SelectObject(hitObject.collider.gameObject);
                        }
                    }
                }
            }
            // Touch held on object, move
            else if (onTouchHold && selectedObject != null)
            {
                Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                if (hitPose != null)
                {
                    selectedObject.transform.position = ((Pose)hitPose).position;
                }
            }

            // Touch released, drop object
            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
                DeselectObject();
            }
        }
    }
}
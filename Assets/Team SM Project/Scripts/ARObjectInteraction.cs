using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

public class ARObjectInteraction : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public PointerEventData pointerEventData;
    private ARRaycastManager _arRaycastManager;
    private GameObject _arSessionOrigin;

    private GameObject selectedObject;  // selected object

    private Vector2 touchPosition;

    private bool interactMode = true;

    public Button interactButton;
    public Button removeButton;

    public Inventory inventory;
    public UIInventory inventoryUI;
    private int selectedSlot = 0;        // selected slot
    public Item selectedItem;           // item in selected slot

    public Text selectedObjectDisplay;
    public Image selectedSlotDisplay;

    //private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {

    }

    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin");
        _arRaycastManager = _arSessionOrigin.GetComponent<ARRaycastManager>();

        interactButton.onClick.AddListener(SetInteractMode);
        removeButton.onClick.AddListener(SetRemoveMode);

        selectedItem = inventory.GetItemList()[selectedSlot];
        selectedObjectDisplay.text = selectedItem.type;
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

    private void SelectObject(GameObject objectToSelect)
    {
        DeselectObject();
        selectedObject = objectToSelect;
        if (selectedObject != null)
        {
            // Display name of selected object
            string type = objectToSelect.name.Remove(objectToSelect.name.IndexOf("(Clone)"), 7);
            selectedObjectDisplay.text = type;
        }
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            // Display name of item in selected slot
            selectedObjectDisplay.text = inventory.GetItemList()[selectedSlot].type;
            selectedObject = null;
        }
    }

    private void PickupObject(GameObject objectToPickup)
    {
        if (objectToPickup != null)
        {
            string type = objectToPickup.name.Remove(objectToPickup.name.IndexOf("(Clone)"), 7);
            // Remove object and put back in inventory
            inventory.PickupItem(inventory.CheckForItem(type).id);
            // ASL?
            Destroy(objectToPickup);
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            // Touch on UI
            if(EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // Graphic Raycast for UI Slot selection
                pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = touchPosition;
                List<RaycastResult> results = new List<RaycastResult>();

                raycaster.Raycast(pointerEventData, results);
                foreach (RaycastResult result in results)
                {
                    if(result.gameObject.GetComponent<UIItem>() != null)
                    {
                        selectedSlot = inventoryUI.GetUIItemList().IndexOf(result.gameObject.GetComponent<UIItem>());
                        selectedItem = inventory.GetItemList()[selectedSlot];
                        selectedObjectDisplay.text = selectedItem.type;
                        selectedSlotDisplay.transform.position = result.gameObject.transform.position;
                    }
                }
            }
            // Touch on World
            else
            {
                // Physics Raycast on touch position
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;
                int layerMask = 1 << 9;
                layerMask = ~layerMask;

                // Check for Object touched, not AR Plane
                if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                {
                    if (hitObject.collider != null)
                    {
                        if (!hitObject.collider.gameObject.name.Contains("ARPlane"))
                        {
                            if (selectedObject == null)
                            {
                                // Select Object at touch position
                                SelectObject(hitObject.collider.gameObject);
                            }
                        }
                    }
                }

                // Touch on AR Plane and no object selected, place
                if (touch.phase == TouchPhase.Began && selectedObject == null)
                {
                    // Interact mode
                    if(interactMode)
                    {
                        // Instantiate new object
                        Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                        if (hitPose != null)
                        {
                            if(!inventory.GetItemList()[selectedSlot].placed)
                            {
                                // Place object
                                ASL.ASLHelper.InstanitateASLObject(inventory.GetItemList()[selectedSlot].type, (((Pose)hitPose).position), (((Pose)hitPose).rotation));
                                inventory.PlaceItem(inventory.GetItemList()[selectedSlot].id);
                            }
                        }
                    }
                    // Pickup mode
                    else
                    {
                        // Pickup object
                        PickupObject(hitObject.collider.gameObject);
                    }
                }
                // Touch on AR Plane and object selected, move
                else if (selectedObject != null)
                {
                    // Interact mode
                    if(interactMode)
                    {
                        Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                        if (hitPose != null)
                        {
                            selectedObject.transform.position = ((Pose)hitPose).position;
                        }
                    }
                    // Pickup mode
                    else
                    {
                        PickupObject(selectedObject);
                    }
                }
            }

            // Touch released, drop object
            if (touch.phase == TouchPhase.Ended)
            {
                DeselectObject();
            }
        }
    }
}
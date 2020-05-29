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

    public GameObject selectedObject;  // selected object

    private Vector2 touchPosition;

    public bool interactMode = true;

    // General Buttons
    public Button interactButton;
    public Button removeButton;

    public Button editNameButton;
    public Button viewStatsButton;

    // Notification
    public GameObject notificationWindow;
    public Button okButton;
    public Text notification;

    // Edit Name
    public GameObject editNameWindow;
    public Text editNameText;
    public Button editNameSaveButton;
    public Button editNameCancelButton;

    // Stats
    public GameObject statsWindow;
    public Gradient barGradient;
    public Slider foodBar;
    public Slider exerciseBar;
    public Slider affectionBar;
    public Image foodFill;
    public Image exerciseFill;
    public Image affectionFill;
    public Text ownerText;

    // Inventory
    public Inventory inventory;
    public UIInventory inventoryUI;
    private int selectedSlot = 0;        // selected slot
    public Item selectedItem;           // item in selected slot

    private string username;
    public Text selectedObjectDisplay;
    public Text selectedNameDisplay;
    public Image selectedSlotDisplay;
    public static bool IsPCPlayer
    {
        get
        {
            return (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor);
        }
    }

    private void Awake()
    {
        username = ASL.GameLiftManager.GetInstance().m_Username;
    }

    private void Start()
    {
        _arSessionOrigin = GameObject.Find("AR Session Origin");
        _arRaycastManager = _arSessionOrigin.GetComponent<ARRaycastManager>();

        // Buttons
        interactButton.onClick.AddListener(SetInteractMode);
        removeButton.onClick.AddListener(SetRemoveMode);
        editNameButton.onClick.AddListener(EditName);
        viewStatsButton.onClick.AddListener(ViewStats);
        editNameSaveButton.onClick.AddListener(SaveEditName);
        editNameCancelButton.onClick.AddListener(CancelEditName);
        okButton.onClick.AddListener(AcknowledgeNotification);

        selectedItem = inventory.GetItem(selectedSlot);
        selectedObjectDisplay.text = selectedItem.type;
        selectedNameDisplay.text = selectedItem.name;
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

    private void EditName()
    {
        if(selectedItem != null)
        {
            editNameText.text = "";
            editNameWindow.SetActive(true);
        }
    }

    private void SaveEditName()
    {
        if(editNameWindow.activeSelf)
        {
            selectedItem.EditName(editNameText.text);
            selectedNameDisplay.text = selectedItem.name;
            editNameWindow.SetActive(false);
        }
    }

    private void CancelEditName()
    {
        if(editNameWindow.activeSelf)
        {
            editNameWindow.SetActive(false);
        }
    }

    private void NotifyUser(string text)
    {
        if(!notificationWindow.activeSelf)
        {
            notification.text = text;
            notificationWindow.SetActive(true);
        }
    }

    private void AcknowledgeNotification()
    {
        if(notificationWindow.activeSelf)
        {
            notificationWindow.SetActive(false);
        }
    }

    private void ViewStats()
    {
        if(selectedItem != null)
        {
            CalculateStats();
            // Activate stats window
            statsWindow.SetActive(!statsWindow.activeSelf);
        }
    }

    private void CalculateStats()
    {
        if(selectedItem != null)
        {
            // Get value
            foodBar.value = selectedItem.stats["Food"];
            exerciseBar.value = selectedItem.stats["Exercise"];
            affectionBar.value = selectedItem.stats["Affection"];
            // Set color of bar as normalized slider value converted to gradient
            foodFill.color = barGradient.Evaluate(foodBar.normalizedValue);
            exerciseFill.color = barGradient.Evaluate(exerciseBar.normalizedValue);
            affectionFill.color = barGradient.Evaluate(affectionBar.normalizedValue);
            // Set owner
            ownerText.text = "Owner: " + selectedItem.owner;
        }
    }

    private void SelectObject(GameObject objectToSelect)
    {
        DeselectObject();
        selectedObject = objectToSelect;
        if (selectedObject != null)
        {
            if(objectToSelect.GetComponent<PetInfo>() != null)
            {
                selectedObjectDisplay.text = objectToSelect.GetComponent<PetInfo>().GetItem().type;
                selectedNameDisplay.text = objectToSelect.GetComponent<PetInfo>().GetItem().name;
            }
            else
            {
                selectedObjectDisplay.text = "ERROR";
                selectedNameDisplay.text = "ERROR";
            }
        }
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            // Display name of item in selected slot
            selectedObjectDisplay.text = inventory.GetItem(selectedSlot).type;
            selectedNameDisplay.text = inventory.GetItem(selectedSlot).name;
            selectedObject = null;
        }
    }

    private void PickupObject(GameObject objectToPickup)
    {
        if (objectToPickup != null)
        {
            if(objectToPickup.GetComponent<PetInfo>() != null)
            {
                // Only pickup if user is the owner
                if(objectToPickup.GetComponent<PetInfo>().GetItem().owner == username)
                {
                    // Remove object and put back in inventory
                    inventory.PickupItem(inventory.CheckForItem(objectToPickup.GetComponent<PetInfo>().GetItem().type).id);
                    objectToPickup.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                    {
                        objectToPickup.gameObject.GetComponent<ASL.ASLObject>().DeleteObject();
                    });
                }
                // Display notification to user
                else
                {
                    NotifyUser("Cannot remove a pet you do not own.");
                }
            }
        }
    }

    void Update()
    {
        if ((Input.touchCount > 0 || (IsPCPlayer && Input.GetMouseButton(0))) && !editNameWindow.activeSelf && !notificationWindow.activeSelf)
        {
            Touch touch;
            int pointerID;
            // Get platform-specific input
            if(IsPCPlayer)
            {
                // PC
                touch = new Touch();
                touchPosition = Input.mousePosition;
                pointerID = -1;
            }
            else
            {
                // Mobile
                touch = Input.GetTouch(0);
                touchPosition = touch.position;
                pointerID = touch.fingerId;
            }

            // Graphic Raycast for UI Slot selection
            pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = touchPosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            // Touch on UI
            foreach (RaycastResult result in results)
            {
                // Raycasted Object has UI Item Component
                if(result.gameObject.GetComponent<UIItem>() != null)
                {
                    // Deselect any currently selected object
                    //DeselectObject();

                    // Get slot as an index to get the Item from the Inventory array
                    selectedSlot = inventoryUI.GetUIItemList().IndexOf(result.gameObject.GetComponent<UIItem>());
                    selectedItem = inventory.GetItem(selectedSlot);

                    // Display name and type of selected pet
                    selectedObjectDisplay.text = selectedItem.type;
                    selectedNameDisplay.text = selectedItem.name;

                    // Move selected UI outline
                    selectedSlotDisplay.transform.position = result.gameObject.transform.position;

                    // If pet is already placed, select the pet in the world
                    /*if(selectedItem.placed)
                    {
                        string searchName = selectedItem.type + "(Clone)";
                        SelectObject(GameObject.Find(searchName));
                    }*/
                }
            }
            // Touch on World
            if(results.Count == 0)
            {
                // Physics Raycast on touch position
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;
                // Cast only on colliders within the "Selectable" and "Platform" layer
                int selectMask = 1 << LayerMask.NameToLayer("Selectable");
                int platformMask = 1 << LayerMask.NameToLayer("Platform");
                int layerMask = selectMask | platformMask;

                if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, layerMask))
                {
                    // Raycast hit
                    if (hitObject.collider != null)
                    {
                        // [DEBUG] Show name of hitObject
                        //selectedNameDisplay.text = hitObject.collider.gameObject.name;

                        // Touch on Platform
                        if (hitObject.collider.gameObject.name.Contains("PlatformPlane"))
                        {
                            // Interact Mode
                            if(interactMode)
                            {
                                // Place Object - No Object Selected
                                if(selectedObject == null)
                                {
                                    // Instantiate new object
                                    Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                                    if (hitPose != null)
                                    {
                                        if(!inventory.GetItem(selectedSlot).placed)
                                        {
                                            // Place object
                                            ASL.ASLHelper.InstanitateASLObject(inventory.GetItem(selectedSlot).type, (((Pose)hitPose).position), (((Pose)hitPose).rotation));
                                            inventory.PlaceItem(inventory.GetItem(selectedSlot).id);
                                        }
                                    }
                                }
                            }
                        }
                        // Touch on Object
                        else
                        {
                            // Interact Mode
                            if(interactMode)
                            {
                                // Select Object - No Object Selected
                                if (selectedObject == null)
                                {
                                    SelectObject(hitObject.collider.gameObject);
                                }
                                // Move Object - Object Currently Selected
                                else
                                {
                                    Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                                    if (hitPose != null)
                                    {
                                        selectedObject.transform.position = ((Pose)hitPose).position;
                                    }
                                }
                            }
                            // Pickup Mode
                            else
                            {
                                // Pickup Object - No Object Selected
                                if(selectedObject == null)
                                {
                                    // Pickup object
                                    PickupObject(hitObject.collider.gameObject);
                                }
                                // Pickup Selected Object - Object Currently Selected
                                else
                                {
                                    PickupObject(selectedObject);
                                }
                            }
                        }
                    }
                }
            }

            // Touch released, drop object
            if ((IsPCPlayer && Input.GetMouseButtonUp(0)) || touch.phase == TouchPhase.Ended)
            {
                DeselectObject();
            }
        }

        // Update stats in realtime if window is displaying
        if(statsWindow.activeSelf)
        {
            CalculateStats();
        }
    }
}
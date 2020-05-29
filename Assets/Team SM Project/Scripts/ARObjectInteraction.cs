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
    public Image selectedModeDisplay;

    public Button editNameButton;
    public Button viewStatsButton;

    // Notification
    public GameObject notificationWindow;
    public Button okButton;
    public Text notification;

    // Edit Name
    public GameObject editNameWindow;
    public InputField editNameInput;
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
    private const int INVENTORY_ROW_WIDTH = 4;
    public Toggle backpack;
    public Inventory inventory;
    public UIInventory inventoryUI;
    private int selectedSlot = 0;        // selected slot
    public Item selectedItem;           // item in selected slot

    private string username;
    public Text selectedObjectDisplay;
    public Text selectedNameDisplay;
    public Image selectedSlotDisplay;

    // PC Player
    public static KeyCode INVENTORY_KEY = KeyCode.Tab;
    public static KeyCode INVENTORY_UP_KEY = KeyCode.UpArrow;
    public static KeyCode INVENTORY_DOWN_KEY = KeyCode.DownArrow;
    public static KeyCode INVENTORY_LEFT_KEY = KeyCode.LeftArrow;
    public static KeyCode INVENTORY_RIGHT_KEY = KeyCode.RightArrow;
    public static KeyCode INTERACT_MODE_KEY = KeyCode.Alpha1;
    public static KeyCode PICKUP_MODE_KEY = KeyCode.Alpha2;
    public static KeyCode EDIT_NAME_KEY = KeyCode.Alpha3;
    public static KeyCode VIEW_STATS_KEY = KeyCode.Alpha4;
    public static KeyCode OK_KEY = KeyCode.Return;
    public static KeyCode CANCEL_KEY = KeyCode.Delete;
    public Image PCCrosshair;
    public static bool IsPCPlayer
    {
        get
        {
            return (Application.platform != RuntimePlatform.Android);
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
        selectedModeDisplay.transform.position = interactButton.gameObject.transform.position;
        DeselectObject();
    }

    private void SetRemoveMode()
    {
        interactMode = false;
        selectedModeDisplay.transform.position = removeButton.gameObject.transform.position;
        DeselectObject();
    }

    private void EditName()
    {
        if(selectedItem != null)
        {
            editNameInput.text = "";
            editNameWindow.SetActive(true);
            editNameInput.ActivateInputField();
        }
    }

    private void SaveEditName()
    {
        if(editNameWindow.activeSelf && editNameText.text != "")
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

    private void SelectSlot(int slot)
    {
        DeselectObject();
        // Set slot to 0 if negative
        if(slot < 0)
        {
            slot = 0;
        }
        // Set slot to max if greater than max
        if(slot > inventory.GetItemList().Count - 1)
        {
            slot = inventory.GetItemList().Count - 1;
        }

        // Select slot and get item
        selectedSlot = slot;
        selectedItem = inventory.GetItem(selectedSlot);

        // Display name and type of selected pet
        selectedObjectDisplay.text = selectedItem.type;
        selectedNameDisplay.text = selectedItem.name;

        // Move selected UI outline
        selectedSlotDisplay.transform.position = inventoryUI.GetUIItemList()[selectedSlot].gameObject.transform.position;
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

    private void PlaceObject(string prefabName, Vector3 pos)
    {
        if(!inventory.GetItem(selectedSlot).placed)
        {
            ASL.ASLHelper.InstanitateASLObject(prefabName, pos, Quaternion.identity);
            inventory.PlaceItem(inventory.GetItem(selectedSlot).id);
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
        // Touch/Click occurs and UI window is not currently up
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
                    // Select Slot
                    SelectSlot(inventoryUI.GetUIItemList().IndexOf(result.gameObject.GetComponent<UIItem>()));
                }
            }
            // Touch on World
            if(results.Count == 0)
            {
                // Physics Raycasts on touch position
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                RaycastHit hitPlatform;
                // Platform Raycast
                int platformMask = 1 << LayerMask.NameToLayer("Platform");
                if (Physics.Raycast(ray, out hitPlatform, Mathf.Infinity, platformMask))
                {
                    // Raycast hit a collider
                    if (hitPlatform.collider != null)
                    {
                        // Interact Mode
                        if(interactMode)
                        {
                            // Place Object - No Object Selected
                            if(selectedObject == null)
                            {
                                PlaceObject(inventory.GetItem(selectedSlot).type, hitPlatform.point);
                            }
                            // Move Selected Object
                            else
                            {
                                selectedObject.transform.position = hitPlatform.point;
                            }
                        }
                    }
                }

                // Selectable Raycast
                RaycastHit hitObject;
                int selectMask = 1 << LayerMask.NameToLayer("Selectable");
                if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, selectMask))
                {
                    // Raycast hit a collider
                    if (hitObject.collider != null)
                    {
                        // Interact Mode
                        if(interactMode)
                        {
                            // Select Object - No Object Selected
                            if (selectedObject == null)
                            {
                                SelectObject(hitObject.collider.gameObject);
                            }
                        }
                        // Pickup Mode
                        else
                        {
                            // Pickup Object - No Object Selected
                            if(selectedObject == null)
                            {
                                PickupObject(hitObject.collider.gameObject);
                            }
                        }
                    }
                }
            }

            // Touch released, drop object
            if (touch.phase == TouchPhase.Ended)
            {
                DeselectObject();
            }
        }

        // Update stats in realtime if window is displaying
        if(statsWindow.activeSelf)
        {
            CalculateStats();
        }

        // PC-specific
        if(IsPCPlayer)
        {
            // Highlight PC crosshair when hovering over selectable object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObject;
            // Raycast only on "Selectable" layer
            int selectMask = 1 << LayerMask.NameToLayer("Selectable");
            if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, selectMask))
            {
                if(hitObject.collider != null)
                {
                    PCCrosshair.color = Color.yellow;
                    // Right click to pickup object
                    if(Input.GetMouseButtonDown(1))
                    {
                        // Pickup Object - No Object Selected
                        if(selectedObject == null)
                        {
                            PickupObject(hitObject.collider.gameObject);
                        }
                        // Pickup Selected Object - Object Currently Selected
                        else
                        {
                            PickupObject(selectedObject);
                        }
                    }
                }
                PCCrosshair.color = Color.yellow;
            }
            else
            {
                PCCrosshair.color = Color.white;
            }

            // Open Backpack with INVENTORY_KEY
            if(Input.GetKeyDown(INVENTORY_KEY))
            {
                backpack.isOn = !backpack.isOn;
            }

            // Navigate Backpack with INVENTORY_UP/DOWN/LEFT/RIGHT_KEY
            if(Input.GetKeyDown(INVENTORY_UP_KEY))
            {
                SelectSlot(selectedSlot - INVENTORY_ROW_WIDTH);
            }
            else if(Input.GetKeyDown(INVENTORY_DOWN_KEY))
            {
                SelectSlot(selectedSlot + INVENTORY_ROW_WIDTH);
            }
            else if(Input.GetKeyDown(INVENTORY_RIGHT_KEY))
            {
                SelectSlot(selectedSlot + 1);
            }
            else if(Input.GetKeyDown(INVENTORY_LEFT_KEY))
            {
                SelectSlot(selectedSlot - 1);
            }

            // Button Hotkeys with 1/2/3/4
            if(Input.GetKeyDown(INTERACT_MODE_KEY))
            {
                SetInteractMode();
            }
            else if(Input.GetKeyDown(PICKUP_MODE_KEY))
            {
                SetRemoveMode();
            }
            else if(Input.GetKeyDown(EDIT_NAME_KEY))
            {
                EditName();
            }
            else if(Input.GetKeyDown(VIEW_STATS_KEY))
            {
                ViewStats();
            }

            // OK/Cancel Hotkey with Enter/Delete
            if(Input.GetKeyDown(OK_KEY))
            {
                SaveEditName();
                AcknowledgeNotification();
            }
            else if(Input.GetKeyDown(CANCEL_KEY))
            {
                CancelEditName();
            }

            // Left click released, drop object
            if(Input.GetMouseButtonUp(0))
            {
                DeselectObject();
            }
        }
    }
}
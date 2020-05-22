using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class SurfaceMenuController : MonoBehaviour
{

    GameObject basePanel;
    GameObject openMenuButton;
    GameObject mainPanel;
    GameObject addSurfacePanel;
    GameObject removeSurfacePanel;
    GameObject moveSurfacePanel;
    private RectTransform basePanelTransform;
    private Vector2 basePanelClosedPos = new Vector2(172.5F, -200.0F);
    private Vector2 basePanelClosedSize = new Vector2(400.0F, 300.0F);
    private Vector2 basePanelOpenPos = new Vector2(690.0F, -200.0F);
    private Vector2 basePanelOpenSize = new Vector2(1600.0F, 300.0F);
    private static ARObjectInteraction petInteractionControls;
    const float basePanelOpenSpeed = 0.2F;
    private bool isOpen;
    private bool showScannedSurfaces;
    private bool addingSurfaces;
    private bool removingSurfaces;
    private bool movingSurfaces;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        showScannedSurfaces = false;
        addingSurfaces = false;
        removingSurfaces = false;
        movingSurfaces = false;
        basePanel = GameObject.Find("PlatformControls").transform.Find("BasePanel").gameObject;
        openMenuButton = basePanel.transform.Find("OpenMenuButton").gameObject;
        addSurfacePanel = basePanel.transform.Find("AddSurfacePanel").gameObject;
        removeSurfacePanel = basePanel.transform.Find("RemoveSurfacePanel").gameObject;
        moveSurfacePanel = basePanel.transform.Find("MoveSurfacePanel").gameObject;
        mainPanel = basePanel.transform.Find("MainPanel").gameObject;
        basePanelTransform = basePanel.GetComponent<RectTransform>();
        petInteractionControls = GameObject.Find("Pet Manager").GetComponent<ARObjectInteraction>();
        if(ARObjectInteraction.IsPCPlayer)
        {
            basePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ARPlaneMeshVisualizer[] planes = (ARPlaneMeshVisualizer[])GameObject.FindObjectsOfType(typeof(ARPlaneMeshVisualizer));
        foreach(ARPlaneMeshVisualizer plane in planes)
        {
            if(showScannedSurfaces)
            {
                plane.enabled = true;
            }
            else
            {
                plane.enabled = false;
            }
            if(removingSurfaces)
            {
                plane.gameObject.layer = 2;
            }
            else
            {
                plane.gameObject.layer = 0;
            }
        }

        if(addingSurfaces || removingSurfaces || movingSurfaces)
        {
            if(petInteractionControls.enabled)
            {
                petInteractionControls.enabled = false;
            }
        }
        else
        {
            petInteractionControls.enabled = true;
        }

        if(addingSurfaces)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = touch.position;

                if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                    RaycastHit hitObject;

                    if (Physics.Raycast(ray, out hitObject, Mathf.Infinity))
                    {
                        if (hitObject.collider != null && hitObject.collider.gameObject.name.Contains("ARPlane"))
                        {
                            hitObject.collider.gameObject.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
                            PlatformController.CreatePlatform(hitObject.collider.gameObject.GetComponent<MeshCollider>().sharedMesh.vertices, hitObject.collider.transform.position, hitObject.collider.transform.rotation);
                        }
                    }
                }
            }
        }
        else if(removingSurfaces)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = touch.position;

                if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                    RaycastHit hitObject;

                    if (Physics.Raycast(ray, out hitObject, Mathf.Infinity))
                    {
                        if (hitObject.collider != null && hitObject.collider.gameObject.name.Contains("PlatformPlane"))
                        {
                            hitObject.collider.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                            {
                                hitObject.collider.gameObject.GetComponent<ASL.ASLObject>().DeleteObject();
                            });
                        }
                    }
                }
            }
        }
        else if(movingSurfaces)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = touch.position;

                if (touch.phase != TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                    RaycastHit hitObject;

                    if (Physics.Raycast(ray, out hitObject, Mathf.Infinity))
                    {
                        if (hitObject.collider != null && hitObject.collider.gameObject.name.Contains("PlatformPlane"))
                        {
                            Pose? hitPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                            if(hitPose != null)
                            {
                                Vector3 newPlanePosition = new Vector3(((Pose)hitPose).position.x, hitObject.collider.transform.position.y, ((Pose)hitPose).position.z);
                                hitObject.collider.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                                {
                                    hitObject.collider.gameObject.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(newPlanePosition);
                                });
                            }
                        }
                    }
                }
            }
        }
    }

    public void OpenMenu()
    {
        LeanTween.moveX(basePanelTransform, basePanelOpenPos.x, basePanelOpenSpeed);
        LeanTween.size(basePanelTransform, basePanelOpenSize, basePanelOpenSpeed).setOnComplete(OnMenuOpened);
        openMenuButton.SetActive(false);
        
    }

    public void OnMenuOpened()
    {
        mainPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        LeanTween.moveX(basePanelTransform, basePanelClosedPos.x, basePanelOpenSpeed);
        LeanTween.size(basePanelTransform, basePanelClosedSize, basePanelOpenSpeed).setOnComplete(OnMenuClosed);
        mainPanel.SetActive(false);
    }

    public void OnMenuClosed()
    {
        openMenuButton.SetActive(true);
    }

    public void AddSurface()
    {
        mainPanel.SetActive(false);
        addSurfacePanel.SetActive(true);
        showScannedSurfaces = true;
        addingSurfaces = true;
    }

    public void RemoveSurface()
    {
        mainPanel.SetActive(false);
        removeSurfacePanel.SetActive(true);
        removingSurfaces = true;
    }

    public void MoveSurface()
    {
        mainPanel.SetActive(false);
        moveSurfacePanel.SetActive(true);
        movingSurfaces = true;
    }

    public void BackToMain()
    {
        addSurfacePanel.SetActive(false);
        removeSurfacePanel.SetActive(false);
        moveSurfacePanel.SetActive(false);
        mainPanel.SetActive(true);
        showScannedSurfaces = false;
        addingSurfaces = false;
        removingSurfaces = false;
        movingSurfaces = false;
    }
}

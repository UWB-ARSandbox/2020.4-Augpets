using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerController : MonoBehaviour
{
    public static string TRANSLATION_AXIS = "Vertical";
    public static string STRAFFE_AXIS = "Horizontal";
    public static KeyCode PAUSE_BUTTON = KeyCode.Escape;
    public static KeyCode SPRINT_KEY = KeyCode.LeftShift;
    public static KeyCode UP_KEY = KeyCode.Space;
    public static KeyCode DOWN_KEY = KeyCode.LeftControl;
    private static float WALK_SPEED = 2.0f;
    private static float SPRINT_SPEED = 10.0f;
    public float movementSpeed;
    public bool isSprinting;
    public bool isMoving;
    public float lookSensitivity = 5.0f;
    public float lookSmoothing = 2.0f;
    public Vector2 lookVector;
    private Vector2 smoothLookVector;
    public Transform playerCamera;
    public GameObject playerCrosshair;
    public static Transform ASLObject;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = this.transform.GetChild(0);
        lookVector = Vector2.zero;
        smoothLookVector = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;
        isSprinting = false;
        isMoving = false;

        playerCamera.gameObject.tag = "MainCamera";
        playerCrosshair.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        rotatePlayer();
        if(isMoving)
        {
            ASLObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                ASLObject.GetComponent<ASL.ASLObject>().SendAndSetLocalPosition(this.transform.position);  
            });
        }
        

        if(Input.GetKeyDown(PAUSE_BUTTON))
        {
            toggleCursorLock();
        }

        if(Input.GetKey(SPRINT_KEY))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

    }

    private void movePlayer()
    {
        movementSpeed = this.isSprinting ? SPRINT_SPEED : WALK_SPEED;
        float translation = Input.GetAxisRaw(TRANSLATION_AXIS) * movementSpeed * Time.deltaTime;
        float straffe = Input.GetAxisRaw(STRAFFE_AXIS) * movementSpeed * Time.deltaTime;
        if(Mathf.Abs(translation) > 0 || Mathf.Abs(straffe) > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        this.transform.Translate(straffe, 0.0F, translation);
    }

    private void rotatePlayer()
    {
        Vector2 mouseDeltaVector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDeltaVector *= lookSensitivity * lookSmoothing;
        smoothLookVector = new Vector2(Mathf.Lerp(smoothLookVector.x, mouseDeltaVector.x, 1.0f / lookSmoothing),
            Mathf.Lerp(smoothLookVector.y, mouseDeltaVector.y, 1.0f / lookSmoothing));
        lookVector += smoothLookVector;

        playerCamera.localRotation = Quaternion.AngleAxis(Mathf.Clamp(-lookVector.y, -90f, 90f), Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(lookVector.x, transform.up);
    }

    private void toggleCursorLock()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public static void getASLObjectReference(GameObject aslObject)
    {
        ASLObject = aslObject.transform;
    }
}

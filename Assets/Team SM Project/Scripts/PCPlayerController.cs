using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerController : MonoBehaviour
{
    public static string TRANSLATION_AXIS = "Vertical";
    public static string STRAFFE_AXIS = "Horizontal";
    public static KeyCode PAUSE_BUTTON = KeyCode.Escape;
    public static KeyCode SPRINT_KEY = KeyCode.LeftShift;
    private static float WALK_SPEED = 5.0f;
    private static float SPRINT_SPEED = 10.0f;
    public float movementSpeed;
    public bool isSprinting;
    public bool isMoving;
    public float lookSensitivity = 5.0f;
    public float lookSmoothing = 2.0f;
    public Vector2 lookVector;
    private Vector2 smoothLookVector;
    public Transform playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = this.transform.GetChild(0);
        lookVector = Vector2.zero;
        smoothLookVector = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;
        isSprinting = false;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.GetComponent<ASL.ASLObject>().SendAndSetLocalRotation(rotatePlayer());
            this.GetComponent<ASL.ASLObject>().SendAndIncrementLocalPosition(movePlayer());  
        });
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

    private Vector3 movePlayer()
    {
        movementSpeed = this.isSprinting ? SPRINT_SPEED : WALK_SPEED;
        float translation = Input.GetAxisRaw(TRANSLATION_AXIS) * movementSpeed * Time.deltaTime;
        float straffe = Input.GetAxisRaw(STRAFFE_AXIS) * movementSpeed * Time.deltaTime;
        if(translation > 0 || straffe > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        return new Vector3(straffe, 0.0f, translation);
    }

    private Quaternion rotatePlayer()
    {
        Vector2 mouseDeltaVector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDeltaVector *= lookSensitivity * lookSmoothing;
        smoothLookVector = new Vector2(Mathf.Lerp(smoothLookVector.x, mouseDeltaVector.x, 1.0f / lookSmoothing),
            Mathf.Lerp(smoothLookVector.y, mouseDeltaVector.y, 1.0f / lookSmoothing));
        lookVector += smoothLookVector;

        playerCamera.localRotation = Quaternion.AngleAxis(-lookVector.y, Vector3.right);
        return Quaternion.AngleAxis(lookVector.x, transform.up);
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
}

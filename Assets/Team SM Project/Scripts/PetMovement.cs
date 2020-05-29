using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    int speed = 1;
    float rotationSpeed = 100f;
    private Inventory inventory;
    private PetInfo info;
    private Item pet;
    private int range = 500;
    private int rotationDirection = 0;
    private Vector3 movementVector = Vector3.zero;
    private Vector3 rotationVector = Vector3.zero;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    

    void Start()
    {
        info = GetComponent<PetInfo>();
        inventory = GameObject.Find("Pet Manager").GetComponent<Inventory>();
        pet = inventory.CheckForItem(this.gameObject.name.Remove(this.gameObject.name.IndexOf("(Clone)"), 7));
        info.TryGetStat("Speed", out speed);
    }
    

    // Update is called once per frame
    void Update()
    {

        if(!isWandering)
        {
            StartCoroutine(Wander());
        }

        if(isRotatingLeft)
        {
            rotationDirection = -1;
        }
        if(isRotatingRight)
        {
            rotationDirection = 1;
        }
        if(!isRotatingRight && !isRotatingLeft)
        {
            rotationVector = Vector3.zero;
        }
        else
        {
            rotationVector = rotationDirection * transform.up * rotationSpeed;
        }

        movementVector = GetWalkingSpeed();

        if(this.transform.rotation.eulerAngles + rotationVector != oldRotation.eulerAngles || this.transform.position + movementVector != oldPosition)
        {
            this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                if(this.transform.rotation.eulerAngles + rotationVector != oldRotation.eulerAngles)
                {
                    this.GetComponent<ASL.ASLObject>().SendAndIncrementLocalRotation(Quaternion.Euler(rotationVector * Time.deltaTime));
                }
                if(this.transform.position + movementVector != oldPosition)
                {
                    this.GetComponent<ASL.ASLObject>().SendAndIncrementLocalPosition(movementVector * Time.deltaTime);
                }
            });
        }
        
        oldPosition = this.transform.position;
        oldRotation = this.transform.rotation;
    }

    private Vector3 GetWalkingSpeed()
    {
        if(isWalking)
        {
            if(pet != null)
            {
                if(pet.movement == "Aerial")
                {
                    return transform.forward * speed * 0.5f;
                }
            }
            if(IsGrounded())
            {
                return transform.forward * speed * 0.5f;
            }
        }
        return Vector3.zero;
    }

    private bool IsGrounded()
    {
        // Physics Linecast down and at an angle pointing forwards
        RaycastHit hitInfo;
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - range, transform.position.z + ((transform.forward * speed).z * 2));
        // Cast only on colliders within the "Selectable" layer
        int selectMask = 1 << LayerMask.NameToLayer("Selectable");
        int platformMask = 1 << LayerMask.NameToLayer("Platform");
        int layerMask = selectMask | platformMask;

        if (Physics.Linecast(transform.position, targetPos, out hitInfo, layerMask))
        {
            if (hitInfo.collider != null)
            {
                if(hitInfo.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotationWait = Random.Range(1, 4);
        int rotationDirection = Random.Range(1, 2);
        int walkWait = Random.Range(1, 4);
        int walkTime = Random.Range(1, 2);
        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotationWait);
        if(rotationDirection == 1)
        {
            isRotatingRight = true;
        }
        else
        {
            isRotatingLeft = true;
        }
        yield return new WaitForSeconds(rotationTime);
        isRotatingLeft = false;
        isRotatingRight = false;

        isWandering = false;
    }
}
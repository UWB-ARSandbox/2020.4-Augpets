using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    float speed = 0.5f;
    float rotationSpeed = 100f;
    private Inventory inventory;
    private Item pet;
    private int range = 500;
    private int rotationDirection = 0;
    private Vector3 movementVector = Vector3.zero;
    private Vector3 rotationVector = Vector3.zero;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    

    void Start()
    {
        inventory = GameObject.Find("Pet Manager").GetComponent<Inventory>();
        pet = inventory.CheckForItem(this.gameObject.name.Remove(this.gameObject.name.IndexOf("(Clone)"), 7));
        if(pet != null)
        {
            //speed = pet.stats["Speed"];
        }
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

        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.GetComponent<ASL.ASLObject>().SendAndIncrementLocalRotation(Quaternion.Euler(rotationVector * Time.deltaTime));
            this.GetComponent<ASL.ASLObject>().SendAndIncrementLocalPosition(movementVector * Time.deltaTime);
            
        });
    }

    private Vector3 GetWalkingSpeed()
    {
        if(isWalking)
        {
            if(pet != null)
            {
                if(pet.stats["Flight"] != 0)
                {
                    return transform.forward * speed;
                }
            }
            // Physics Linecast straight down
            RaycastHit hitInfo;
            Vector3 startPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - range, transform.position.z + ((transform.forward * speed).z * 2));
            if (Physics.Linecast(transform.position, targetPos, out hitInfo))
            {
                if (hitInfo.collider != null)
                {
                    if (hitInfo.collider.gameObject.name.Contains("PlatformPlane") || hitInfo.collider.gameObject.name.Contains("ARPlane"))
                    {
                        return transform.forward * speed;
                    }
                }
            }
        }
        return Vector3.zero;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    float speed = 0.5f;
    float rotationSpeed = 100f;
    private int rotationDirection = 0;
    private Vector3 movementVector = Vector3.zero;
    private Vector3 rotationVector = Vector3.zero;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    

    void Start()
    {
        
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
        if(isWalking)
        {
            movementVector = transform.forward * speed;
        }
        else
        {
            movementVector = Vector3.zero;
        }

        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.GetComponent<ASL.ASLObject>().SendAndIncrementLocalRotation(Quaternion.Euler(rotationVector * Time.deltaTime));
            this.GetComponent<ASL.ASLObject>().SendAndIncrementLocalPosition(movementVector * Time.deltaTime);
            
        });
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

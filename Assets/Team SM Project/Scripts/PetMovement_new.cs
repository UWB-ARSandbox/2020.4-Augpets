using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement_new : MonoBehaviour
{

    private PetInfo info;
    private ARObjectInteraction interaction;
    private bool isSelected;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 nextPosition;
    private Quaternion nextRotation;
    public enum MovementType
    {
        Wander,
        Jump,
        Fall,
        Idle
    }

    public bool isWandering;
    public bool isJumping;
    public bool isIdle;
    public float wanderTime;
    public int movementSpeed;
    public float viewLength = 10.0F;
    public bool floorBelow = false;
    public bool floorInFront = false;
    public bool platformAbove = false;
    public bool platformBelow = false;

    public float maxMovementTime = 3.0F;
    public float maxPauseTime = 4.0F;

    public Vector3 platformAbovePosition;
    public MovementType currentMovementType;
    private Coroutine currentCoroutine;

    void Start()
    {
        lastPosition = this.transform.position;
        lastRotation = this.transform.rotation;
        nextPosition = lastPosition;
        nextRotation = lastRotation;
        info = this.GetComponent<PetInfo>();
        currentMovementType = MovementType.Idle;
        info.TryGetStat("Speed", out movementSpeed);
        interaction = GameObject.Find("Pet Manager").GetComponent<ARObjectInteraction>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectSurfaces();
        if(currentMovementType == MovementType.Idle)
        {
            if(floorBelow)
            {
                currentMovementType = MovementType.Wander;
            }
            if(platformAbove)
            {
                currentMovementType = MovementType.Jump;
            }
        }
        if(interaction.selectedObject == this.gameObject)
        {
            StopCoroutine(currentCoroutine);
            isWandering = false;
            nextPosition = this.transform.position;
            isSelected = true;
        }
        else
        {
            isSelected = false;
            switch(currentMovementType)
            {
                case MovementType.Wander: 
                    if(!isWandering)
                    {
                        currentCoroutine = StartCoroutine(Wander());
                    }
                    break;
                case MovementType.Jump:
                    if(!isJumping)
                    {
                        //StartCoroutine(Jump(this.transform.position, platformAbovePosition));
                    }
                    break;
                case MovementType.Fall: break;
                case MovementType.Idle: break;
                default: break;
            }
            UpdatePosition();
        }
        
    }


    IEnumerator Wander()
    {
        isWandering = true;
        while(wanderTime > 0 && currentMovementType == MovementType.Wander)
        {
            nextPosition = lastPosition + (this.transform.forward * movementSpeed * Time.deltaTime);
            wanderTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(Random.Range(0.0F, maxPauseTime));
        wanderTime = Random.Range(0.0F, maxMovementTime);
        Rotate();
        
    }

    private void Rotate()
    {
        if(!floorInFront)
        {
            nextRotation = Quaternion.Euler(lastRotation.x, lastRotation.y + 180, lastRotation.z);
        }
        else
        {
            nextRotation = Quaternion.Euler(0.0F, Random.Range(0, 360), 0.0F);
        }
        
        this.currentMovementType = MovementType.Idle;
        isWandering = false;
    }

    // Not yet fully functioning
    /*
    IEnumerator Jump(Vector3 startPosition, Vector3 endPosition)
    {
        isJumping = true;
        float arcAmount = 8f;
        float heightOfShot = (platformAbovePosition.y - this.transform.position.y) + 3;
        Vector3 newVel = new Vector3();
        // Find the direction vector without the y-component
        Vector3 direction = new Vector3(endPosition.x, 0f, endPosition.z) - new Vector3(startPosition.x, 0f, startPosition.z);
        // Find the distance between the two points (without the y-component)
        float range = direction.magnitude;
 
        // Find unit direction of motion without the y component
        Vector3 unitDirection = direction.normalized;
        // Find the max height
   
        float maxYPos = startPosition.y + heightOfShot;
   
        // if it has, switch the height to match a 45 degree launch angle
        if (range / 2f > maxYPos)
            maxYPos = range / arcAmount;
        //fix bug when shooting on tower
        if (maxYPos - startPosition.y <= 0)
        {
            maxYPos = startPosition.y + 2f;
        }
        //fix bug caused if we can't shoot higher than target
        if (maxYPos - endPosition.y <= 0)
        {
            maxYPos = endPosition.y + 2f;
        }
        // find the initial velocity in y direction
        newVel.y = Mathf.Sqrt(-2.0f * Physics.gravity.y * (maxYPos - startPosition.y));
        // find the total time by adding up the parts of the trajectory
        // time to reach the max
        float timeToMax = Mathf.Sqrt(-2.0f * (maxYPos - startPosition.y) / Physics.gravity.y);
        // time to return to y-target
        float timeToTargetY = Mathf.Sqrt(-2.0f * (maxYPos - endPosition.y) / Physics.gravity.y);
        // add them up to find the total flight time
        float totalFlightTime = timeToMax + timeToTargetY;
        // find the magnitude of the initial velocity in the xz direction
        float horizontalVelocityMagnitude = range / totalFlightTime;
        // use the unit direction to find the x and z components of initial velocity
        newVel.x = horizontalVelocityMagnitude * unitDirection.x;
        newVel.z = horizontalVelocityMagnitude * unitDirection.z;
   
        float elapse_time = 0;
        while (elapse_time < totalFlightTime)
        {
            this.transform.Translate(newVel.x * Time.deltaTime, (newVel.y - (Physics.gravity.y * elapse_time)) * Time.deltaTime, newVel.z * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }
        isJumping = false;
        currentMovementType = MovementType.Idle;
    }
    */
    private void DetectSurfaces()
    {
        float petHeight = this.GetComponent<Collider>().bounds.size.y / 2;
        float petWidth = this.GetComponent<Collider>().bounds.size.x / 2;
        Vector3 floorBelow = new Vector3(transform.position.x, transform.position.y - (petHeight + 1F), transform.position.z);
        Vector3 floorInFront = new Vector3(transform.position.x, transform.position.y - (petHeight + 1F), transform.position.z + transform.forward.z * (petWidth + 0.1F));
        Vector3 platformAbove = new Vector3(transform.position.x, transform.position.y + viewLength, transform.position.z + ((transform.forward * movementSpeed * viewLength).z));
        Vector3 platformBelow = new Vector3(transform.position.x, transform.position.y - viewLength, transform.position.z + ((transform.forward * movementSpeed * viewLength).z));


        int layerMask = 1 << LayerMask.NameToLayer("Platform");
        RaycastHit hit;
        Transform floor = null;

        
        if (Physics.Linecast(transform.position, floorBelow, out hit, layerMask))
        {
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.floorBelow = true;
                    floor = hit.collider.transform;
                }
                
            }
           
        }
        else
        {
            this.floorBelow = false;
            //StopCoroutine("Wander");
        }

        if(Physics.Linecast(transform.position, floorInFront, out hit, layerMask))
        {
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.floorInFront = true;
                    floor = hit.collider.transform;
                }
            }
        }
        else
        {
            this.floorInFront = false;
            StopCoroutine(currentCoroutine);
            isWandering = false;
            this.nextPosition = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z + -this.transform.forward.z * Time.deltaTime * movementSpeed);
        }
        

        
        /*if(Physics.Linecast(platformAbove, transform.position, out hit, layerMask))
        {
            if(hit.collider != null && (floor != null && hit.collider.transform != floor) && hit.collider.transform.position.y > this.transform.position)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.platformAbove = true;
                    platformAbovePosition = hit.point;
                }
                else
                {
                    this.platformAbove = false;
                }
            }
        }*/

        // Currently can't check for a platform to jump down to.
        /*if(Physics.Linecast(transform.position, platformBelow, out hit, layerMask))
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.platformAbove = true;
                }
            }
        }*/

    }

    private void UpdatePosition()
    {
        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(nextRotation);

            if(lastPosition != nextPosition)
            {
                this.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(nextPosition);
            }
        });
        lastPosition = nextPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement_new : MonoBehaviour
{

    private PetInfo info;
    private ARObjectInteraction interaction;
    private bool isSelected;
    private Vector3 lastPosition;
    private Vector3 initalJumpPosition;
    private Vector3 initialFallPosition;
    private Vector3 targetPosition;
    private Quaternion lastRotation;
    private Vector3 nextPosition;
    private Quaternion nextRotation;
    public enum MovementType
    {
        Wander,
        Rotate,
        Jump,
        Fall,
        Idle
    }

    public float wanderTime;
    public float pauseTime;
    public int movementSpeed;
    public float viewLength = 8.0F;
    public bool floorBelow = false;
    public bool floorInFront = false;
    public bool jumpablePlatformAbove = false;
    public bool platformAbove = false;
    public bool platformBelow = false;
    public bool canJump = false;
    public bool canFall = false;
    public bool canFly = false;

    public float fallSpeed = 8.0F;
    public float jumpSpeed = 5.0F;
    public float flySpeed = 1.5F;

    public float timeOfLastJump = 0;

    public float maxMovementTime = 3.0F;
    public float maxPauseTime = 4.0F;

    public float minTimeBetweenJump = 3.0F;

    public Vector3 jumpEndPosition;
    public Vector3 midJumpPosition;

    public Vector3 fallEndPosition;
    public Vector3 midFallPosition;
    public MovementType currentMovementType;
    private float speedMultiplier = 0.25F;

    void Start()
    {
        info = this.GetComponent<PetInfo>();
        currentMovementType = MovementType.Idle;
        info.TryGetStat("Speed", out movementSpeed);
        canFly = info.itemRef.movement.Equals("Aerial");
        interaction = GameObject.Find("Pet Manager").GetComponent<ARObjectInteraction>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(info.owner != interaction.username)
        {
            return;
        }

        if(currentMovementType == MovementType.Idle || currentMovementType == MovementType.Wander)
        {
            DetectSurfaces();
        }
        if(interaction.selectedObject == this.gameObject)
        {
            nextPosition = this.transform.position;
        }
        else
        {
            if(canJump && currentMovementType != MovementType.Jump && currentMovementType != MovementType.Fall)
            {
                initalJumpPosition = this.transform.position;
                currentMovementType = MovementType.Jump;
                targetPosition = midJumpPosition;
            }
            if(canFall && currentMovementType != MovementType.Fall && currentMovementType != MovementType.Jump)
            {
                initialFallPosition = this.transform.position;
                currentMovementType = MovementType.Fall;
                targetPosition = midFallPosition;
            }
            float step = Time.deltaTime;
            switch(currentMovementType)
            {      
                case MovementType.Wander: 

                    if(floorBelow)
                    {
                        if(floorInFront)
                        {
                            if(wanderTime > 0)
                            {
                                nextPosition = this.transform.position + (this.transform.forward * speedMultiplier * movementSpeed * Time.deltaTime);
                                wanderTime -= Time.deltaTime;
                            }
                            else
                            {
                                
                                pauseTime = Random.Range(0, maxPauseTime);
                                currentMovementType = MovementType.Idle;
                            }
                        }
                        else
                        {
                            Rotate(Random.Range(0, 360));
                            nextPosition = this.transform.position;
                        }
                    }
                    else
                    {
                        nextPosition = this.transform.position;
                    }
                    
                    break;

                case MovementType.Idle:
                    if(pauseTime > 0)
                    {
                        pauseTime -= Time.deltaTime;
                    }
                    else
                    {
                        wanderTime = Random.Range(0, maxMovementTime);
                        Rotate(Random.Range(0, 360));
                        currentMovementType = MovementType.Wander;

                    }
                    break;

                case MovementType.Jump:
                    if(canFly)
                    {
                        step *= flySpeed;
                    }
                    else
                    {
                        step *= jumpSpeed;
                    }
                    if(this.transform.position != targetPosition)
                    {
                        
                        nextPosition = Vector3.MoveTowards(transform.position, targetPosition, step);
                        //LeanTween.move(this.gameObject, midJumpPosition, 0.5F);
                    }
                    else if(this.transform.position == jumpEndPosition)
                    {
                        nextPosition = this.transform.position;
                        currentMovementType = MovementType.Idle;
                        timeOfLastJump = Time.time;
                    }
                    else
                    {
                        if(targetPosition == midJumpPosition)
                        {
                            targetPosition = jumpEndPosition;
                        }
                    }
                    break;
                case MovementType.Fall:
                    if(canFly)
                    {
                        step *= flySpeed;
                    }
                    else
                    {
                        step *= fallSpeed;
                    }
                    if(this.transform.position != targetPosition)
                    {
                        
                        nextPosition = Vector3.MoveTowards(transform.position, targetPosition, step);
                        //LeanTween.move(this.gameObject, midJumpPosition, 0.5F);
                    }
                    else if(this.transform.position == fallEndPosition)
                    {
                        nextPosition = this.transform.position;
                        currentMovementType = MovementType.Idle;
                        timeOfLastJump = Time.time;
                    }
                    else
                    {
                        if(targetPosition == midFallPosition)
                        {
                            targetPosition = fallEndPosition;
                        }
                    }
                    break;
                case MovementType.Rotate:
                    
                    break;
                
                default: break;
            }
        }
        
        UpdatePosition();
        lastPosition = this.transform.position;
    }


    private void Rotate(float degrees)
    {
        nextRotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y + degrees, this.transform.rotation.z);
        nextPosition = this.transform.position;
    }

    private void DetectSurfaces()
    {
        float petHeight = this.GetComponent<Collider>().bounds.size.y;
        float petWidth = this.GetComponent<Collider>().bounds.size.z;
        Vector3 petPosition = this.transform.position + new Vector3(0, petHeight / 2, 0);
        Vector3 floorBelow = petPosition + new Vector3(0, -((petHeight / 2) + 0.01F), 0);
        Vector3 floorInFront = floorBelow + (this.transform.forward * (petWidth / 2));
        Vector3 jumpablePlatformAbove = petPosition + (this.transform.forward * (petWidth / 3)) + new Vector3(0, petHeight * viewLength, 0);
        Vector3 platformAbove = petPosition + new Vector3(0, petHeight * viewLength, 0);
        Vector3 platformBelowStart = this.transform.position + this.transform.forward * ((petWidth / 2)) - new Vector3(0, 0.2F, 0);
        Vector3 platformBelowEnd = platformBelowStart + new Vector3(0, -viewLength, 0);

        GameObject jumpablePlatform = null;
        GameObject abovePlatform = null;
        GameObject floor = null;
        GameObject belowPlatform = null;
        bool sufficientTimeSinceLastJump = Time.time - timeOfLastJump >= minTimeBetweenJump;


        int layerMask = 1 << LayerMask.NameToLayer("Platform");
        RaycastHit hit;

        Debug.DrawLine(petPosition, floorBelow, Color.green);
        if (Physics.Linecast(petPosition, floorBelow, out hit, layerMask))
        {
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.floorBelow = true;
                    floor = hit.collider.gameObject;
                }
                
            }
           
        }
        else
        {
            this.floorBelow = false;
        }

        Debug.DrawLine(petPosition, floorInFront, Color.green);
        if(Physics.Linecast(petPosition, floorInFront, out hit, layerMask))
        {
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.floorInFront = true;
                }
            }
        }
        else
        {
            this.floorInFront = false;
            this.nextPosition = new Vector3(lastPosition.x, lastPosition.y, lastPosition.z + -this.transform.forward.z * Time.deltaTime * movementSpeed);
        }
        
        
        Debug.DrawLine(jumpablePlatformAbove, petPosition, Color.green);
        if(Physics.Linecast(jumpablePlatformAbove, petPosition, out hit, layerMask))
        {
            if(hit.collider != null && (floor != null && hit.collider.transform != floor))
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.jumpablePlatformAbove = true;
                    midJumpPosition = new Vector3(petPosition.x, hit.point.y, petPosition.z);
                    jumpEndPosition = hit.point + this.transform.forward * 0.25F;
                    jumpablePlatform = hit.collider.gameObject;
                }
                else
                {
                    this.jumpablePlatformAbove = false;
                }
            }
            else
            {
                this.jumpablePlatformAbove = false;
            }
        }
        else
        {
            this.jumpablePlatformAbove = false;
        }

        Debug.DrawLine(platformAbove, petPosition, Color.green);
        if(Physics.Linecast(platformAbove, petPosition, out hit, layerMask))
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    this.platformAbove = true;
                    abovePlatform = hit.collider.gameObject;
                }
                else
                {
                    this.platformAbove = false;
                }
            }
            else
            {
                this.platformAbove = false;
            }
        }
        else
        {
            this.platformAbove = false;
        }

        this.canJump = sufficientTimeSinceLastJump && jumpablePlatform && (abovePlatform != jumpablePlatform);

        Debug.DrawLine(platformBelowStart, platformBelowEnd, Color.green);
        if(Physics.Linecast(platformBelowStart, platformBelowEnd, out hit, layerMask))
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.name.Contains("PlatformPlane"))
                {
                    if(hit.collider.gameObject != floor)
                    {
                        this.platformBelow = true;
                        belowPlatform = hit.collider.gameObject;
                        midFallPosition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                        fallEndPosition = hit.point + this.transform.forward * 0.25F;
                    }
                    else
                    {
                        this.platformBelow = false;
                    }
                }
                else
                {
                    this.platformBelow = false;
                }
            }
            else
            {
                this.platformBelow = false;
            }
        }

        this.canFall = sufficientTimeSinceLastJump && !this.floorInFront && platformBelow;
    }

    private void UpdatePosition()
    {
        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            if(this.transform.rotation != nextRotation)
            {
                this.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(nextRotation);
            }
            
            if(nextPosition != lastPosition)
            {
                this.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(nextPosition);
            }
            
        });
    }
}

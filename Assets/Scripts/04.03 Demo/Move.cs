using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool isMoving = false;
    Vector3 direction;
    float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMoving)
        {
            direction = _randomDirection();
            isMoving = true;
        }
        else
        {
            this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                this.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(this.transform.position + (direction * speed));
            });
        }
        /*if(Mathf.Abs(this.transform.position.x) > 4.5 || Mathf.Abs(this.transform.position.z) > 4.5)
        {
            this.isMoving = false;
        }*/
    }

    Vector3 _randomDirection()
    {
        return (new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f))).normalized;
    }
}

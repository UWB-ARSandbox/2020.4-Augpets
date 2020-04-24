using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Player p;
    // Start is called before the first frame update
    void Start()
    {
        p = this.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(p.playerNumber == 1)
            {
                GameObject.Instantiate(p.playerObject, _randomPosition(), this.transform.rotation, this.transform);
            }
        }
    }

    Vector3 _randomPosition()
    {
        return new Vector3(Random.Range(-4.5f, 4.5f), 0.5f, Random.Range(-4.5f, 4.5f));
    }
}

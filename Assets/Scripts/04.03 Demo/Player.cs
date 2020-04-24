using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static int playerCount = 0;
    public int playerNumber;
    public GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        playerCount++;
        this.playerNumber = playerCount;
        if(playerNumber == 1)
        {
            this.playerObject = (GameObject)Resources.Load("Prefabs/P1Object"); 
        }
        else
        {
            this.playerObject = (GameObject)Resources.Load("Prefabs/P2Object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

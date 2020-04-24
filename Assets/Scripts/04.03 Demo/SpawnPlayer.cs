using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    static GameObject playerPrefab;

    void Start()
    {
        if(playerPrefab == null)
        {
            playerPrefab = (GameObject)Resources.Load("Prefabs/Player");
        }

        GameObject.Instantiate(playerPrefab, playerPrefab.transform.position, playerPrefab.transform.rotation, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

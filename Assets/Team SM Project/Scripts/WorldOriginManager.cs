using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldOriginManager : MonoBehaviour
{
    GameObject worldOrigin;
    private void Start()
    {

        if(ASL.GameLiftManager.GetInstance().AmLowestPeer())
        {
            ASL.ASLHelper.InstanitateASLObject("WorldOrigin", Vector3.zero, Quaternion.identity, "", "", SpawnWorldOrigin);
        }
        
    }

    private static void SpawnWorldOrigin(GameObject worldOrigin)
    {
        worldOrigin.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            ASL.ASLHelper.CreateARCoreCloudAnchor(Pose.identity, worldOrigin.GetComponent<ASL.ASLObject>(), _waitForAllUsersToResolve:false);
        });
    }

}

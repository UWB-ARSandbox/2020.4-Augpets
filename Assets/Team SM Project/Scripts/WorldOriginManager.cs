using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldOriginManager : MonoBehaviour
{

    public static void CreateWorldOrigin()
    {
        ASL.ASLHelper.InstanitateASLObject("WorldOrigin", Vector3.zero, Quaternion.identity, "", "", SpawnWorldOrigin);
    }

    // Update is called once per frame
    private static void SpawnWorldOrigin(GameObject worldOrigin)
    {
        worldOrigin.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            ASL.ASLHelper.CreateARCoreCloudAnchor(new Pose(), null, _setWorldOrigin:true);
        });
    }
}

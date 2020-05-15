using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlatformController : MonoBehaviour
{
    private Toggle platformControlsToggle;
    private static ARObjectInteraction objectControls;
    private static Pose? cloudAnchorPose;

    private static Vector3[] verticesOfSelectedPlane;
    public bool IsSelecting
    {
        get
        {
            if(platformControlsToggle == null)
            {
                platformControlsToggle = GameObject.Find("PlatformControls").transform.Find("Panel").Find("Toggle").GetComponent<Toggle>();
            }
            return platformControlsToggle.isOn;
        }
        
    }
    private void Awake()
    {
        objectControls = GameObject.Find("Pets").GetComponent<ARObjectInteraction>();
        platformControlsToggle = GameObject.Find("PlatformControls").transform.Find("Panel").Find("Toggle").GetComponent<Toggle>();
        verticesOfSelectedPlane = new Vector3[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSelecting)
        {
            if(objectControls.enabled)
            {
                objectControls.enabled = false;
            }
            CheckForSelection();
        }
        else
        {
            objectControls.enabled = true;
        }
    }

    void CheckForSelection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                cloudAnchorPose = ASL.ARWorldOriginHelper.GetInstance().Raycast(touchPosition);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject, Mathf.Infinity))
                {
                    if (hitObject.collider != null && hitObject.collider.gameObject.name.Contains("ARPlane"))
                    {
                        CreatePlatform(hitObject.collider.gameObject.GetComponent<MeshCollider>().sharedMesh.vertices, hitObject.collider.transform.position, hitObject.collider.transform.rotation);
                    }
                }
            }
        }
    }

    void CreatePlatform(Vector3[] vertices, Vector3 arPlanePosition, Quaternion arPlaneRotation)
    {
        verticesOfSelectedPlane = vertices;
        ASL.ASLHelper.InstanitateASLObject("PlatformPlane", arPlanePosition, arPlaneRotation, "", "", SendMeshVertices, null, UpdateMesh);
    }

    private static void SendMeshVertices(GameObject platform)
    {
        
        platform.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
  
            //ASL.ASLHelper.CreateARCoreCloudAnchor(cloudAnchorPose, platform.GetComponent<ASL.ASLObject>(), null, false, false);
            platform.GetComponent<ASL.ASLObject>().SendFloatArray(Vector3ArrayToFloatArray(verticesOfSelectedPlane));
        });
    }


    public static void UpdateMesh(string _id, float[] f)
    { 
        ASL.ASLObject platform;
        if(ASL.ASLHelper.m_ASLObjects.TryGetValue(_id, out platform))
        {
            Vector3[] vertices = FloatArrayToVector3Array(f);
            Mesh platformMesh = new Mesh();
            platform.GetComponent<MeshFilter>().mesh = platformMesh;
            platformMesh.vertices = vertices;
            platformMesh.triangles = Triangulator.Triangulate(vertices.Length);

            platformMesh.RecalculateNormals();

            // The shared mesh is set to null first because otherwise
            // Unity throws a fit and the mesh is never updated.
            platform.GetComponent<MeshCollider>().sharedMesh = null;
            platform.GetComponent<MeshCollider>().sharedMesh = platformMesh;
        }
        platform.GetComponent<MeshRenderer>().enabled = true;
    }

    public static float[] Vector3ArrayToFloatArray(Vector3[] vectors)
    {
        // Here we multiply the vector length by 2 since the float array will contain the x and z positions
        // of every point, and add 1 because we need to know the y position, but since it's the same for
        // all vertices we shouldn't send it more than once.
        float[] result = new float [(vectors.Length * 2) + 1];
        result[0] = vectors[0].y;
        for(int i = 0; i < vectors.Length; i++)
        {
            result[(i * 2) + 1] = vectors[i].x;
            result[(i * 2) + 2] = vectors[i].z;
        }
        return result;
    }

    public static Vector3[] FloatArrayToVector3Array(float[] floats)
    {
        // Check the Vector3ArrayToFloatArray to see why this has to be so confusing.
        // This is just reversing the process from that method.
        Vector3[] result = new Vector3[(floats.Length - 1) / 2];
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = new Vector3(floats[(i * 2) + 1], floats[0], floats[(i * 2) + 2]);
        }
        return result;
    }

    public static void debugPlatformCreated(GameObject _worldOriginVisualizationObject, Pose _spawnLocation)
    {
        Debug.Log("Platform Created!");
    }
}

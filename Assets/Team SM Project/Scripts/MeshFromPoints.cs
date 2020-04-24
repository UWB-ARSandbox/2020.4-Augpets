using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFromPoints : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();

        vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0)
        };
    }

    public static Vector2[] getV2FromV3(Vector3[] v3)
    {
        return System.Array.ConvertAll(v3, v3ToV2);
    }

    public static Vector3[] getV3FromV2(Vector2[] v2)
    {
        return System.Array.ConvertAll(v2, v2ToV3);
    }

    public static Vector2 v3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static Vector3 v2ToV3(Vector2 v2)
    {
        return new Vector3(v2.x, v2.y, 0);
    }

}

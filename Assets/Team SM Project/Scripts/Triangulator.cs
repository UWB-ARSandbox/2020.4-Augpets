using UnityEngine;
using System.Collections.Generic;
 
 // Based on code from habrador.com/tutorials/math/10-triangulation
public class Triangulator
{ 
    public static int[] Triangulate(int edgePointCount)
    {
        List<int> triangles = new List<int>();

        for(int i = 2; i < edgePointCount; i++)
        {
            triangles.Add(0);
            triangles.Add(i - 1);
            triangles.Add(i);
        }

        return triangles.ToArray();
    }
}


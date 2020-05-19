using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldMesh
{
    public static bool Vector3ListContains(List<Vector3> vList, Vector3 u, float margin=0.01f)
    {
        foreach(Vector3 v in vList)
        {
            float d2 = (v - u).sqrMagnitude;
            if(d2 <= margin)
                return true;
        }

        return false;
    }
    public static int Vector3ListIndexOf(List<Vector3> vList, Vector3 u, float margin=0.01f)
    {
        for(int i = 0; i < vList.Count; ++i)
        {
            float d2 = (vList[i] - u).sqrMagnitude;
            if(d2 <= margin)
                return i;
        }

        return -1;
    }

    public static void Weld(Mesh mesh, float margin=0.00001f, bool verbose=false)
    {
        Vector3[] verts = mesh.vertices;
        int[] tris = mesh.triangles;
        Vector2[] uvs = mesh.uv;

        List<Vector3> vDict = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        int pointCollisions = 0; // stats.
        for(int i = 0; i < verts.Length; ++i)
        {
            // add new vertices if they are not close to others.
            bool contains = Vector3ListContains(vDict, verts[i], margin);
            if(contains)
                pointCollisions++;
            else
            {
                vDict.Add(verts[i]);
                newUVs.Add(uvs[i]);
            }
        }
        if(verbose) // helps us know the compression stats.
            Debug.Log(string.Format("POINT COLLISIONS: {0}", pointCollisions));

        // remap the triangles so they use the new vertex points.
        List<int> newTris = new List<int>();
        for(int i = 0; i < tris.Length; ++i)
        {
            int t = tris[i];
            Vector3 v = verts[t];
            int newT = Vector3ListIndexOf(vDict, v, margin);
            newTris.Add(newT);
        }
        
        // create the new mesh from the current object.
        mesh.Clear();
        mesh.vertices = vDict.ToArray();
        mesh.triangles = newTris.ToArray();
        mesh.uv = newUVs.ToArray();
        mesh.RecalculateNormals();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh))]
public class ClothRender
{
    private Mesh mesh;
    private Transform transform;

    public ClothRender(Mesh Mesh, Transform Transform)
    {
        this.mesh = Mesh;
        this.transform = Transform;
    }

    public void UpdateVerticesFromPhysics(Vector3[] wVerts)
    {
        if(this.mesh == null)
        {
            Debug.LogError("Mesh is null.");
            return;
        }

        Vector3[] verts = this.mesh.vertices;

        if(wVerts.Length != verts.Length)
        {
            Debug.LogError(string.Format("Number of vertices must be constant; using {0} when there are {1}", wVerts.Length, verts.Length));
            return;
        }

        // convert world verts. to mesh verts.
        Vector3[] newVerts = new Vector3[wVerts.Length];
        for(int i = 0; i < newVerts.Length; ++i)
        {
            newVerts[i] = transform.InverseTransformPoint(wVerts[i]);
        }

        this.mesh.vertices = newVerts;
        this.mesh.RecalculateNormals();
    }

    public Vector3[] GetWorldVertices()
    {
        Vector3[] meshVerts = this.mesh.vertices;
        Vector3[] worldVerts = new Vector3[meshVerts.Length];
        for(int i = 0; i < worldVerts.Length; ++i)
        {
            worldVerts[i] = transform.TransformPoint(meshVerts[i]);
        }
        return worldVerts;
    }
}

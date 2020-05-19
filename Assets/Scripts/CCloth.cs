using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshRenderer))]
public class CCloth : MonoBehaviour
{
    ClothPhysics clothPhysics;
    ClothRender clothRender;

    [SerializeField]
    [Range(0.1f, 100.0f)]
    private float mass = 1.0f;

    [SerializeField]
    [Range(0,1)]
    private float springK = 1.0f;

    [SerializeField]
    [Range(0,1)]
    private float damping = 0.01f;

    [SerializeField]
    private float timestep = 0.004f;

    [SerializeField]
    private List<int> pinnedVertices;

    [SerializeField]
    private List<CollisionConstraint> collisionConstraints;

    [SerializeField]
    private int collisionConstraintSteps = 1;

    [SerializeField]
    private int substeps = 1;

    // DEBUGGING.
    [SerializeField]
    private bool drawConstraints = true;

    [SerializeField]
    private bool drawDynamics = true;

    [SerializeField]
    private bool drawCollisionConstraints = true;

    [SerializeField]
    private bool recordRuntime = true;

    private float avgTime = 0.0f;

    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        WeldMesh.Weld(mesh, 0.000001f, true);
        float fullMass = this.mass * mesh.vertices.Length;
        this.clothRender = new ClothRender(mesh, this.transform);
        Vector3[] worldVerts = this.clothRender.GetWorldVertices();
        int[] meshTriangles = mesh.triangles;
        this.clothPhysics = new ClothPhysics(
                            worldVerts, meshTriangles, fullMass, this.springK, 
                            Physics.gravity, 1.0f-this.damping, this.timestep/this.substeps, 
                            this.pinnedVertices,
                            this.collisionConstraints,
                            this.collisionConstraintSteps
                            );
    }

    void FixedUpdate()
    {
        Stopwatch st = Stopwatch.StartNew();
        for(int i = 0; i < this.substeps; ++i)
            this.clothPhysics.PhysicsStep();
        st.Stop();

        avgTime = avgTime * 0.9f + st.ElapsedMilliseconds*0.1f;
    }

    void Update()
    {
        PointMass[] points = this.clothPhysics.points;
        Vector3[] pVerts = new Vector3[points.Length];
        for(int i = 0; i < pVerts.Length; ++i) // TODO: this copying could be avoided with pntrs.
            pVerts[i] = points[i].position;
       this.clothRender.UpdateVerticesFromPhysics(pVerts);
       
        if(this.recordRuntime)
            UnityEngine.Debug.Log(string.Format("average time: {0}", avgTime));
    }
    
    void OnDrawGizmos()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            if(this.drawConstraints)
                DrawConstraints();
            if(this.drawDynamics)
                DrawDynamics();
        }

        if(this.drawCollisionConstraints)
            DrawCollisionConstraints();
    }

    void DrawConstraints()
    {
        HashSet<Constraint> constraints = this.clothPhysics.contraints;
        foreach(Constraint c in constraints)
        {
            UnityEngine.Debug.DrawLine(c.pA.position, c.pB.position, Color.red);
        }
    }

    void DrawDynamics()
    {
        foreach(PointMass p in this.clothPhysics.points)
        {
            UnityEngine.Debug.DrawLine(p.position, p.lastPosition, Color.blue);
        }
    }

    void DrawCollisionConstraints()
    {
        foreach(CollisionConstraint c in this.collisionConstraints)
        {
            c.Draw(Color.green);
        }
    }
}

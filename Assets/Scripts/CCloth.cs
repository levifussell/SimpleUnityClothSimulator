using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshRenderer))]
public class CCloth : MonoBehaviour
{
    ClothPhysics clothPhysics;
    ClothRender clothRender;

    [SerializeField]
    [Range(0.1f, 100.0f)]
    private float mass;

    [SerializeField]
    [Range(0,1)]
    private float springK;

    [SerializeField]
    [Range(0,1)]
    private float damping;

    [SerializeField]
    private float timestep;

    [SerializeField]
    private int constraintSteps = 5;

    [SerializeField]
    private List<int> pinnedVertices;

    [SerializeField]
    private bool debug;

    [SerializeField]
    private List<CollisionConstraint> collisionConstraints;

    [SerializeField]
    private int collisionConstraintSteps = 1;

    [SerializeField]
    private int substeps = 1;


    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        float fullMass = this.mass * mesh.vertices.Length;
        this.clothRender = new ClothRender(mesh, this.transform);
        Vector3[] worldVerts = this.clothRender.GetWorldVertices();
        int[] meshTriangles = mesh.triangles;
        this.clothPhysics = new ClothPhysics(
                            worldVerts, meshTriangles, fullMass, this.springK, 
                            Physics.gravity, 1.0f-this.damping, this.timestep/this.substeps, 
                            this.constraintSteps, this.pinnedVertices,
                            this.collisionConstraints,
                            this.collisionConstraintSteps
                            );
    }

    void FixedUpdate()
    {
        for(int i = 0; i < this.substeps; ++i)
            this.clothPhysics.PhysicsStep();
    }

    void Update()
    {
        PointMass[] points = this.clothPhysics.points;
        Vector3[] pVerts = new Vector3[points.Length];
        for(int i = 0; i < pVerts.Length; ++i) // TODO: this copying could be avoided with pntrs.
            pVerts[i] = points[i].position;
       this.clothRender.UpdateVerticesFromPhysics(pVerts);
    }
    
    void OnDrawGizmos()
    {
       if(this.debug)
       {
        //    DrawConstraints();
            if(UnityEditor.EditorApplication.isPlaying)
                DrawDynamics();
            DrawCollisionConstraints();
       }
    }

    void DrawConstraints()
    {
        HashSet<Constraint> constraints = this.clothPhysics.contraints;
        foreach(Constraint c in constraints)
        {
            Debug.DrawLine(c.pA.position, c.pB.position, Color.red);
        }
    }

    void DrawDynamics()
    {
        foreach(PointMass p in this.clothPhysics.points)
        {
            Debug.DrawLine(p.position, p.lastPosition, Color.blue);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothPhysics
{
    public PointMass[] points {get; private set;}
    // private Constraint[] constraints;
    public HashSet<Constraint> contraints { get; private set;}

    private float mass;
    public float massPerPoint{ get { return points == null ? 0.0f : mass / points.Length; }} 

    private float springK;

    private Vector3 gravity;

    private float timestep;
    private float timestepSqrd;
    
    private float damping;

    private int constraintSteps;

    private List<CollisionConstraint> collisionConstraints { get; }

    private int collisionConstraintSteps;

    public ClothPhysics(
            Vector3[] WorldVertPositions, int[] MeshTriangles, float Mass, 
            float SpringK, Vector3 Gravity, float Damping, float Timestep, 
            int ConstraintSteps, List<int> PinnedVertices, 
            List<CollisionConstraint> CollisionConstraints,
            int CollisionConstraintSteps
            )
    {
        this.mass = Mass;
        this.springK = SpringK;
        this.gravity = Gravity;
        this.damping = Damping;
        this.timestep = Timestep;
        this.timestepSqrd = this.timestep * this.timestep;
        this.constraintSteps = ConstraintSteps;
        this.collisionConstraints = CollisionConstraints;
        this.collisionConstraintSteps = CollisionConstraintSteps;

        this.ExtractPointsAndEdgesFromMesh(WorldVertPositions, MeshTriangles, PinnedVertices);
    }

    private void ExtractPointsAndEdgesFromMesh(Vector3[] worldVertPositions, int[] meshTriangles, List<int> pinnedVertices=null)
    {
        if(worldVertPositions == null || meshTriangles == null)
        {
            Debug.LogError("Null mesh data passed.");
            return;
        }

        // create point masses.
        points = new PointMass[worldVertPositions.Length];
        for(int i = 0; i < worldVertPositions.Length; ++i)
        {
            float m = this.massPerPoint;
            if(pinnedVertices != null && pinnedVertices.Contains(i))
                m = 0.0f;
            points[i] = new PointMass(worldVertPositions[i], m);
        }

        // create spring constraints according to mesh triangles.
        contraints = new HashSet<Constraint>();
        for(int i = 0; i < meshTriangles.Length; i += 3)
        {
            for(int j = 0, k = 1; j < 3; ++j, k = (k+1)%3)
            {
                int tA = meshTriangles[i+j];
                int tB = meshTriangles[i+k];
                float length = (points[tA].position - points[tB].position).magnitude;
                SpringConstraint sAB = new SpringConstraint(points[tA], points[tB], length, springK);
                contraints.Add(sAB);
            }
        }

        // TODO: bending constraints (will be a little more complicated...)
    }

    private void ResetPointForces()
    {
        foreach(PointMass p in points)
            p.ResetForces();
    }

    private void ApplyGravity()
    {
        foreach(PointMass p in points)
            p.AddForce(this.gravity * p.mass);
    }

    private void ApplyConstraints()
    {
        foreach(Constraint c in this.contraints)
            c.ApplyConstraint();
    }

    private void VerletIntegration()
    {
        foreach(PointMass p in points)
        {
            // p.lastStepCollidedScalar = 0.0f; // reset collision scalar.
            Vector3 x_1 = p.position;
            Vector3 x_0 = p.lastPosition;
            Vector3 a_1 = p.ComputeAcceleration();
            Vector3 x_2 = x_1 + this.damping*(x_1 - x_0) + a_1 * this.timestepSqrd; // x_2 = x_1 + (x_1 - x_0) + a * dt^2 (Verlet integration)
            p.position = x_2;
            p.lastPosition = x_1;
        }
    }

    private void ApplySelfCollisions()
    {
        // TODO.
    }

    private void ApplyCollisionConstraints()
    {
        foreach(CollisionConstraint c in this.collisionConstraints)
            c.ApplyConstraint(this.points);
    }

    public void PhysicsStep()
    {
        this.ResetPointForces(); 
        this.ApplyGravity();
        this.VerletIntegration();

        for(int i = 0; i < collisionConstraintSteps; ++i)
        {
            this.ApplyCollisionConstraints();
            this.ApplyConstraints(); // every collision should be followed by a constraint satisfaction.
        }

        for(int i = 0; i < constraintSteps; ++i)
            this.ApplyConstraints();

        this.ApplySelfCollisions();
    }
}





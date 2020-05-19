using UnityEngine;
using System.Collections.Generic;

public class PointMass
{
    private Vector3 _position;
    public Vector3 position { get; set; }
    public Vector3 lastPosition { get; set; }
    private Vector3 startPosition;
    public Vector3 forces { get; private set; }

    public float mass { get; private set; }
    public float invMass { get { return mass == 0 ? 0 : 1.0f/mass; } }

    public float pinConstraintScalar { get; private set; }

    public float lastStepCollidedScalar { get; set; }

    public PointMass(Vector3 StartPosition, float Mass)
    {
        this.startPosition = StartPosition;
        this.mass = Mass;
        this.pinConstraintScalar = this.invMass == 0.0 ? 0.0f : 1.0f;
        this.ResetPosition();
    }

    public void ResetPosition()
    {
        this.position = this.startPosition;
        this.lastPosition = this.startPosition; // we assume... (should still be okay)
    }

    public void ResetForces()
    {
        this.forces = Vector3.zero;
    }

    public void AddForce(Vector3 f)
    {
        this.forces += f;
    }

    public Vector3 ComputeAcceleration()
    {
        return this.forces * this.invMass; // we don't put this as a property so no one thinks it is cached.
    }

    public override bool Equals(object obj)
    {
        if(obj == null | !obj.GetType().Equals(this.GetType()))
            return false;
        PointMass pObj = (PointMass)obj;
        return this.position == pObj.position && this.mass == pObj.mass;
    }

    public override int GetHashCode()
    {
        return System.Tuple.Create(this.position.x, this.position.y, this.position.z).GetHashCode();
    }

    public static void Test()
    {
        Debug.Log("RUNNING TESTS FOR: PointMass");

        PointMass a = new PointMass(new Vector3(1, 0, 1), 1.0f);
        PointMass b = new PointMass(new Vector3(1, 0, 1), 1.0f);
        PointMass c = new PointMass(new Vector3(0, 1, 1), 1.0f);
        bool test1 = a.Equals(b) && !c.Equals(b);
        if(!test1)
            Debug.LogError("\tTEST 1 FAILED.");

        bool test2 = a.GetHashCode() == b.GetHashCode() && c.GetHashCode() != a.GetHashCode();
        if(!test2)
            Debug.LogError("\tTEST 2 FAILED.");

        int size = 20;
        float fS = (float)size;
        float hS = (float)size/2.0f;
        // PointMass[,,] vol3D = new PointMass[size,size,size];
        HashSet<PointMass> pms = new HashSet<PointMass>();
        bool test3 = true;
        for(int i = 0; i < size; ++i)
        {
            for(int j = 0; j < size; ++j)
            {
                for(int k = 0; k < size; ++k)
                {
                    Vector3 position = new Vector3(i/fS - hS, j/fS - hS, k/fS - hS);
                    PointMass p = new PointMass(position, 0.01f);
                    if(!pms.Add(p))
                    {
                        Debug.Log(string.Format("\tTEST 3 collision at: {0}", position));
                        test3 = false;
                        break;
                    }
                }
            }
        }
        if(!test3)
            Debug.LogError("\tTEST 3 FAILED.");

        Debug.Log("\tTESTS COMPLETE.");
    }
}

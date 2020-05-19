using System;
using UnityEngine;

public abstract class Constraint
{
    public PointMass pA {get; protected set;}
    public PointMass pB {get; protected set;}
    public Constraint(PointMass P_a, PointMass P_b)
    {
        pA = P_a;
        pB = P_b;
    }

    public abstract void ApplyConstraint();

    public override bool Equals(object obj)
    {
        if(obj == null || !obj.GetType().Equals(this.GetType()))
            return false;

        Constraint other = obj as Constraint;
        return other.pA == this.pA && other.pB == this.pB;
    }

    public override int GetHashCode()
    {
        return Tuple.Create(pA, pB).GetHashCode();
    }
}

public class SpringConstraint : Constraint
{
    private float k;
    private float length;
    public SpringConstraint(PointMass P_a, PointMass P_b, float Length, float K)
        : base(P_a, P_b)
    {
        length = Length;
        k = K;
    }

    public override void ApplyConstraint()
    {
        Vector3 dV = this.pA.position - this.pB.position;
        float d = dV.magnitude;
        Vector3 dVn = dV / d;
        float lOffset = length - d;
        Vector3 correctionA = lOffset * dVn;
        Vector3 correctionB = lOffset * -dVn;
        // float forceScalar = (this.pA.invMass == 0.0 || this.pB.invMass == 0.0 || this.pA.lastStepCollidedScalar == 1.0f || this.pB.lastStepCollidedScalar == 1.0f) ? 1.0f : 0.5f;
        // this.pA.position += this.k*correctionA * forceScalar * this.pA.pinConstraintScalar * (1.0f - this.pA.lastStepCollidedScalar);
        // this.pB.position += this.k*correctionB * forceScalar * this.pB.pinConstraintScalar * (1.0f - this.pB.lastStepCollidedScalar);
        float forceScalar = (this.pA.invMass == 0.0 || this.pB.invMass == 0.0 || this.pA.lastStepCollidedScalar == 1.0f || this.pB.lastStepCollidedScalar == 1.0f) ? 1.0f : 0.5f;
        this.pA.position += this.k*correctionA * forceScalar * this.pA.pinConstraintScalar;
        this.pB.position += this.k*correctionB * forceScalar * this.pB.pinConstraintScalar;
    }
}

public class SpringConstraintSqrtApprox : Constraint
// removes 'costly' sqrt function w/ first-order approx.
{
    private float k;
    private float length;
    public SpringConstraintSqrtApprox(PointMass P_a, PointMass P_b, float Length, float K)
        : base(P_a, P_b)
    {
        length = Length;
        k = K;
    }

    public override void ApplyConstraint()
    {
        Vector3 dV = this.pA.position - this.pB.position;
        dV *= this.length*this.length/(Vector3.Dot(dV, dV) + this.length*this.length) - 0.5f;
        this.pA.position += this.k*dV * this.pA.pinConstraintScalar;
        this.pB.position -= this.k*dV * this.pB.pinConstraintScalar;
    }
}
using UnityEngine;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionConstraint : CollisionConstraint
{
    // [SerializeField]
    public Vector3 position {get; set;}
    [SerializeField]
    public float radius;
    public float radiusSqrd { get; private set; }

    [SerializeField]
    private float friction;

    // public SphereCollisionConstraint(Vector3 Position, float Radius)
    // {
    //     this.position = Position;
    //     this.radius = Radius;
    //     this.radiusSqrd = this.radius * this.radius;
    // }

    public void Start()
    {
        this.position = this.transform.position;
        this.radiusSqrd = this.radius * this.radius;
    }

    public void Update()
    {
        this.position = this.transform.position;
    }

    public override void ApplyConstraint(PointMass[] points)
    {
        foreach(PointMass p in points)
        {
            Vector3 diff_x1 = this.position - p.position;
            if(diff_x1.sqrMagnitude < this.radiusSqrd)
            {
                p.position = this.position + -diff_x1.normalized * this.radius;

                // also correct the last_position (if inside) so that we do not get crazy normal velocities.
                Vector3 diff_x0 = this.position - p.lastPosition;
                if(diff_x0.sqrMagnitude < this.radiusSqrd)
                {
                    p.lastPosition = p.position;
                    // p.lastStepCollidedScalar = 1.0f;
                }
                else
                {
                    Vector3 correction = p.lastPosition - p.position;
                    p.position = p.lastPosition + correction * (1.0f - this.friction);
                }
            }

        }
    }

    public override void Draw(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(this.position, this.radius);
    }
}
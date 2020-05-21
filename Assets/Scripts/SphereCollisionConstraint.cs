using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionConstraint : CollisionConstraint
{
    public Vector3 position {get; set;}
    [SerializeField]
    public float radius;
    public float radiusSqrd { get; private set; }

    [SerializeField]
    public Vector3 offset;

    // public SphereCollisionConstraint(Vector3 Position, float Radius)
    // {
    //     this.position = Position;
    //     this.radius = Radius;
    //     this.radiusSqrd = this.radius * this.radius;
    // }

    public void Start()
    {
        this.radiusSqrd = this.radius * this.radius;
    }

    public override void UpdatePosition()
    {
        this.position = this.transform.position + this.offset;
    }

    public override bool ApplyConstraint(ref Vector3 position)
    {
        Vector3 diff_x1 = this.position - position;
        if(diff_x1.sqrMagnitude < this.radiusSqrd)
        {
            position = this.position + -diff_x1.normalized * this.radius;
            return true;
        }

        return false;
    }

    public override void Draw(Color color)
    {
        base.Draw(color);
        Gizmos.DrawWireSphere(this.position, this.radius);
    }
}
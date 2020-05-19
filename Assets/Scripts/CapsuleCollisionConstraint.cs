using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCollisionConstraint : CollisionConstraint
{
    public Vector3 position { get; set; }
    [SerializeField]
    private float height = 1;
    [SerializeField]
    private float radius = 0.5f;
    private float radiusSqrd;

    // [SerializeField]
    // private float friction = 1.0f;

    // a capsule contains two sphere colliders.
    private SphereCollisionConstraint[] sphereConstraints;

    private const int CYLINDER_WIRE_MESH_RESOLUTION = 8;

    void Start()
    {
        this.position = this.transform.position;        
        this.radiusSqrd = this.radius*this.radius;

        sphereConstraints = new SphereCollisionConstraint[2];
        for(int i = 0; i < 2; ++i)
        {
            sphereConstraints[i] = this.gameObject.AddComponent<SphereCollisionConstraint>();
            sphereConstraints[i].radius = this.radius;
        }
    }

    // Update is called once per frame
    public override void UpdatePosition()
    {
        this.position = this.transform.position;        
    }

    public override void Update()
    {
        base.Update();
        this.sphereConstraints[0].offset = this.GetTop() - this.position;
        this.sphereConstraints[1].offset = this.GetBottom() - this.position;
    }

    public override bool ApplyConstraint(ref Vector3 position)
    {
        // resolve sphere collision constraints.
        foreach(SphereCollisionConstraint sC in this.sphereConstraints)
        {
            if(sC.ApplyConstraint(ref position))
                return true;
        }

        // resolve (open-ended) cyclinder collision constraints.
        Vector3 a = GetTop();
        Vector3 b = GetBottom();
        Vector3 c = position;
        Vector3 ab = b-a;
        Vector3 ba = -ab;
        Vector3 ac = c-a;
        Vector3 bc = c-b;
        float d_a = Vector3.Dot(ab, ac);
        float d_b = Vector3.Dot(ba, bc);
        if(d_a >= 0.0f && d_b >= 0.0f) // we are between the top and bottom.
        {
            Vector3 proj_c = d_a * ab / ab.sqrMagnitude + a; // project AC onto AB and move to world cords.
            Vector3 diff = proj_c - c;
            if(diff.sqrMagnitude <= this.radiusSqrd) // collision check.
            {
                position = proj_c + -diff.normalized * this.radius;
                return true;
                // Vector3 correction = p.lastPosition - p.position;
                // p.position = p.lastPosition + correction * (1.0f - this.friction);
            }
        }

        return false;
    }

    public Vector3 GetTop()
    {
        return this.position + this.height * this.transform.up * 0.5f;
    }

    public Vector3 GetBottom()
    {
        return this.position - this.height * this.transform.up * 0.5f;
    }

    public override void Draw(Color color)
    {
        base.Draw(color);

        Gizmos.DrawWireSphere(this.GetTop(), this.radius); // we don't do this with the SphereCC so that it works in edit mode.
        Gizmos.DrawWireSphere(this.GetBottom(), this.radius);
        for(int i = 0; i < CYLINDER_WIRE_MESH_RESOLUTION; ++i)
        {
            Quaternion rot = Quaternion.AngleAxis(360.0f/CYLINDER_WIRE_MESH_RESOLUTION*i, this.transform.up);
            Gizmos.DrawLine(this.GetTop() + rot * this.transform.right * this.radius, 
                            this.GetBottom() + rot * this.transform.right * this.radius);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class CollisionConstraint : MonoBehaviour
{
    [SerializeField]
    private float friction = 1.0f;
    // public CollisionConstraint() {}
    public abstract bool ApplyConstraint(ref Vector3 position);
    // public abstract void ApplyConstraint(PointMass[] points);
    public void ApplyConstraint(PointMass[] points)
    {
        foreach(PointMass p in points)
        {
            Vector3 pPos = p.position;
            if(ApplyConstraint(ref pPos))
            {
                p.position = pPos;
                Vector3 pPosLast = p.lastPosition;
                if(!ApplyConstraint(ref pPosLast))
                {
                    // apply friction.
                    Vector3 correction = p.lastPosition - p.position;
                    p.position = p.lastPosition + (1.0f - this.friction) * correction;
                }
            }
        }
    }

    public virtual void Update()
    {
        this.UpdatePosition();
    }

    public abstract void UpdatePosition();

    public virtual void Draw(Color color)
    {
        Gizmos.color = color;
        this.UpdatePosition();
    }
}

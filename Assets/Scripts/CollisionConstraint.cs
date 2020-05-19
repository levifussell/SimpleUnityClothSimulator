using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class CollisionConstraint : MonoBehaviour
{
    // public CollisionConstraint() {}
    public abstract void ApplyConstraint(PointMass[] points);

    public abstract void Draw(Color color);
}
